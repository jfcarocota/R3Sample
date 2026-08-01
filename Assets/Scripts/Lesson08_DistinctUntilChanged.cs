using R3;
using UnityEngine;

public class Lesson08_DistinctUntilChanged : MonoBehaviour
{
    private ReactiveProperty<int> _vida = new ReactiveProperty<int>(100);
    private DisposableBag _subs;

    void Start()
    {
        // Sin DistinctUntilChanged: avisa aunque el valor sea el mismo
        _vida.Subscribe(v => Debug.Log($"[Normal] Vida: {v}"))
            .AddTo(ref _subs);

        // Con DistinctUntilChanged: solo avisa si realmente cambió
        _vida.DistinctUntilChanged()
            .Subscribe(v => Debug.Log($"[Distinct] Vida: {v}"))
            .AddTo(ref _subs);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            _vida.Value -= 10;  // cambia el valor

        if (Input.GetKeyDown(KeyCode.X))
            _vida.Value = _vida.Value;  // asigna el mismo valor
    }

    void OnDestroy()
    {
        _subs.Dispose();
        _vida.Dispose();
    }
}
