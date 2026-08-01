using R3;
using UnityEngine;

public class Lesson10_Merge : MonoBehaviour
{
    private Subject<string> _ataquePrimario   = new Subject<string>();
    private Subject<string> _ataqueSecundario = new Subject<string>();
    private Subject<string> _ataqueEspecial   = new Subject<string>();
    private DisposableBag _subs;

    void Start()
    {
        // Sin Merge: tendrías que suscribirte a cada uno por separado
        // Con Merge: un solo Subscribe maneja todos
        _ataquePrimario
            .Merge(_ataqueSecundario)
            .Merge(_ataqueEspecial)
            .Subscribe(ataque => Debug.Log($"Ataque recibido: {ataque}"))
            .AddTo(ref _subs);

        // También puedes filtrar sobre el stream combinado
        _ataquePrimario
            .Merge(_ataqueSecundario)
            .Merge(_ataqueEspecial)
            .Where(ataque => ataque == "especial")
            .Subscribe(_ => Debug.Log("Activando efecto especial!"))
            .AddTo(ref _subs);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) _ataquePrimario.OnNext("primario");
        if (Input.GetKeyDown(KeyCode.S)) _ataqueSecundario.OnNext("secundario");
        if (Input.GetKeyDown(KeyCode.E)) _ataqueEspecial.OnNext("especial");
    }

    void OnDestroy()
    {
        _subs.Dispose();
        _ataquePrimario.Dispose();
        _ataqueSecundario.Dispose();
        _ataqueEspecial.Dispose();
    }
}
