using R3;
using UnityEngine;

public class Lesson11_CombineLatest : MonoBehaviour
{
    private ReactiveProperty<int> _vida = new ReactiveProperty<int>(100);
    private ReactiveProperty<int> _mana = new ReactiveProperty<int>(50);
    private ReactiveProperty<bool> _tieneEspada = new ReactiveProperty<bool>(false);
    private DisposableBag _subs;

    void Start()
    {
        // CombineLatest emite cuando CUALQUIERA de los dos cambia
        // y combina sus últimos valores
        _vida.CombineLatest(_mana, (v, m) => $"Vida: {v} | Mana: {m}")
            .Subscribe(stats => Debug.Log(stats))
            .AddTo(ref _subs);

        // Caso práctico: habilidad disponible solo si tiene mana Y espada
        _mana.CombineLatest(_tieneEspada, (m, espada) => m >= 20 && espada)
            .DistinctUntilChanged()
            .Subscribe(disponible => Debug.Log($"Habilidad disponible: {disponible}"))
            .AddTo(ref _subs);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) _vida.Value      -= 10;
        if (Input.GetKeyDown(KeyCode.M)) _mana.Value      -= 10;
        if (Input.GetKeyDown(KeyCode.E)) _tieneEspada.Value = !_tieneEspada.Value;
    }

    void OnDestroy()
    {
        _subs.Dispose();
        _vida.Dispose();
        _mana.Dispose();
        _tieneEspada.Dispose();
    }
}
