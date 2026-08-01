using R3;
using UnityEngine;

public class Lesson09_ThrottleDebounce : MonoBehaviour
{
    private Subject<int> _clicks = new Subject<int>();
    private int _clickCount = 0;
    private DisposableBag _subs;

    void Start()
    {
        // Throttle: emite una vez por segundo sin importar cuántos clicks lleguen
        // Útil para: cooldowns, rate limiting, guardar datos
        _clicks
            .Throttle(TimeSpan.FromSeconds(1), TimeProvider.System)
            .Subscribe(n => Debug.Log($"[Throttle] Procesó click #{n}"))
            .AddTo(ref _subs);

        // Debounce: espera 0.5s sin eventos, luego emite el último
        // Útil para: búsquedas en tiempo real, validación de inputs
        _clicks
            .Debounce(TimeSpan.FromSeconds(0.5f), TimeProvider.System)
            .Subscribe(n => Debug.Log($"[Debounce] Usuario terminó de clickear, último: #{n}"))
            .AddTo(ref _subs);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _clickCount++;
            Debug.Log($"Click #{_clickCount}");
            _clicks.OnNext(_clickCount);
        }
    }

    void OnDestroy()
    {
        _subs.Dispose();
        _clicks.Dispose();
    }
}
