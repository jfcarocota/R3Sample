using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

public class Lesson15_UniTask : MonoBehaviour
{
    private ReactiveProperty<int> _nivel = new ReactiveProperty<int>(1);
    private Subject<Unit> _onAtaque      = new Subject<Unit>();
    private DisposableBag _subs;

    void Start()
    {
        // Observable -> UniTask: espera hasta que el nivel llegue a 5
        EsperarNivel5().Forget();

        // UniTask -> Observable: mete una carga async dentro de un stream
        _onAtaque
            .Select(_ => CargarEfecto().ToObservable())
            .Switch()
            .Subscribe(efecto => Debug.Log($"Efecto listo: {efecto}"))
            .AddTo(ref _subs);
    }

    async UniTaskVoid EsperarNivel5()
    {
        // FirstAsync espera el primer valor que cumpla la condicion
        int nivel = await _nivel.Where(n => n >= 5).FirstAsync();
        Debug.Log($"Llegaste al nivel {nivel}! Desbloqueando contenido...");
    }

    async UniTask<string> CargarEfecto()
    {
        Debug.Log("Cargando efecto...");
        await UniTask.Delay(1000); // simula carga
        return "explosion_vfx";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) _nivel.Value++;
        if (Input.GetKeyDown(KeyCode.A)) _onAtaque.OnNext(Unit.Default);
    }

    void OnDestroy()
    {
        _subs.Dispose();
        _nivel.Dispose();
        _onAtaque.Dispose();
    }
}
