using R3;
using UnityEngine;

public class Lesson12_Switch : MonoBehaviour
{
    private Subject<string> _busqueda = new Subject<string>();
    private int _busquedaNum = 0;
    private DisposableBag _subs;

    void Start()
    {
        // Sin Switch: cada búsqueda corre en paralelo
        // Si llegan en orden distinto al esperado, muestras datos viejos
        _busqueda
            .Select(termino => SimularBusqueda(termino))  // crea un nuevo Observable por cada termino
            .Switch()                                      // cancela el anterior, solo procesa el último
            .Subscribe(resultado => Debug.Log($"Resultado: {resultado}"))
            .AddTo(ref _subs);
    }

    // Simula una búsqueda que tarda un tiempo variable
    private Observable<string> SimularBusqueda(string termino)
    {
        Debug.Log($"Iniciando búsqueda: '{termino}'");

        // En un juego real esto sería una llamada a servidor o base de datos
        return Observable.Timer(TimeSpan.FromSeconds(1), TimeProvider.System)
            .Select(_ => $"Resultados para '{termino}'");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) _busqueda.OnNext("espada");
        if (Input.GetKeyDown(KeyCode.Alpha2)) _busqueda.OnNext("escudo");
        if (Input.GetKeyDown(KeyCode.Alpha3)) _busqueda.OnNext("pocion");
    }

    void OnDestroy()
    {
        _subs.Dispose();
        _busqueda.Dispose();
    }
}
