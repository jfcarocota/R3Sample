using R3;
using UnityEngine;

public class Lesson07_Skip : MonoBehaviour
{
    private ReactiveProperty<int> _score = new ReactiveProperty<int>(0);
    private DisposableBag _subs;

    void Start()
    {
        // Sin Skip: avisa desde el principio (incluye el valor inicial 0)
        _score.Subscribe(s => Debug.Log($"[Sin Skip] Score: {s}"))
            .AddTo(ref _subs);

        // Skip(1): ignora el valor inicial, solo reacciona a cambios reales
        _score.Skip(1)
            .Subscribe(s => Debug.Log($"[Skip 1] Score: {s}"))
            .AddTo(ref _subs);

        // Skip(3): ignora los primeros 3 valores
        _score.Skip(3)
            .Subscribe(s => Debug.Log($"[Skip 3] Score: {s}"))
            .AddTo(ref _subs);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            _score.Value += 10;
        }
    }

    void OnDestroy()
    {
        _subs.Dispose();
        _score.Dispose();
    }
}
