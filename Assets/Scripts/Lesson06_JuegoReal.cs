using R3;
using UnityEngine;

// --- JUGADOR ---
// Tiene estado (vida) y emite eventos (muerte)
public class Jugador : MonoBehaviour
{
    public ReactiveProperty<int> Vida = new ReactiveProperty<int>(100);
    public Subject<Unit> OnMuerte = new Subject<Unit>();

    private DisposableBag _subs;

    void Start()
    {
        // Cuando la vida llega a 0, emite el evento de muerte (una sola vez)
        Vida.Where(v => v <= 0)
            .Take(1)
            .Subscribe(_ => OnMuerte.OnNext(Unit.Default))
            .AddTo(ref _subs);
    }

    public void RecibirDanio(int cantidad)
    {
        Vida.Value = Mathf.Max(0, Vida.Value - cantidad);
    }

    void OnDestroy()
    {
        _subs.Dispose();
        Vida.Dispose();
        OnMuerte.Dispose();
    }
}

// --- ENEMIGO ---
// Ataca al jugador y reacciona a su muerte
public class Enemigo : MonoBehaviour
{
    [SerializeField] private Jugador jugador;

    private DisposableBag _subs;

    void Start()
    {
        // Escucha cuando el jugador muere
        jugador.OnMuerte
            .Subscribe(_ => Debug.Log("Enemigo: el jugador murió, dejo de atacar"))
            .AddTo(ref _subs);

        // Escucha la vida del jugador para saber cuándo celebrar
        jugador.Vida
            .Where(v => v <= 25)
            .Subscribe(v => Debug.Log($"Enemigo: casi lo tengo! Vida restante: {v}"))
            .AddTo(ref _subs);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jugador.RecibirDanio(30);
            Debug.Log($"Enemigo atacó! Vida del jugador: {jugador.Vida.Value}");
        }
    }

    void OnDestroy()
    {
        _subs.Dispose();
    }
}
