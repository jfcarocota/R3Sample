using R3;
using UnityEngine;

public class Lesson04_DisposableBag : MonoBehaviour
{
    private ReactiveProperty<int> _vida = new ReactiveProperty<int>(100);

    // DisposableBag es un contenedor de suscripciones
    private DisposableBag _subs;

    void Start()
    {
        _vida.Subscribe(v => Debug.Log($"HP: {v}"))
            .AddTo(ref _subs);  // agrega la suscripcion al bag

        _vida.Where(v => v <= 0)
            .Subscribe(_ => Debug.Log("Muerto!"))
            .AddTo(ref _subs);  // otra suscripcion al mismo bag
    }

    void Update()
    {
        // Presiona D para dañar, espera ver los logs
        if (Input.GetKeyDown(KeyCode.D))
            _vida.Value -= 10;
    }

    void OnDestroy()
    {
        // Cuando el GameObject se destruye, cancela TODO de golpe
        _subs.Dispose();
        _vida.Dispose();
    }
}
