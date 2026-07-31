using R3;
using UnityEngine;

public class Lesson05_Subject : MonoBehaviour
{
    // Subject<T> emite eventos del tipo T
    private Subject<string> _onAtaque = new Subject<string>();

    private DisposableBag _subs;

    void Start()
    {
        // Alguien escucha el evento
        _onAtaque
            .Subscribe(tipo => Debug.Log($"Ataque recibido: {tipo}"))
            .AddTo(ref _subs);

        // Puedes filtrar igual que con ReactiveProperty
        _onAtaque
            .Where(tipo => tipo == "fuego")
            .Subscribe(_ => Debug.Log("Inmune al fuego!"))
            .AddTo(ref _subs);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            _onAtaque.OnNext("espada");   // emite el evento manualmente

        if (Input.GetKeyDown(KeyCode.F))
            _onAtaque.OnNext("fuego");
    }

    void OnDestroy()
    {
        _subs.Dispose();
        _onAtaque.Dispose();
    }
}
