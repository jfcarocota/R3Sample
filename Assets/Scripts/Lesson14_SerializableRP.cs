using R3;
using UnityEngine;

public class Lesson14_SerializableRP : MonoBehaviour
{
    // Aparece en el Inspector — puedes editarla en Play Mode
    [SerializeField]
    private SerializableReactiveProperty<int> _nivel = new SerializableReactiveProperty<int>(1);

    [SerializeField]
    private SerializableReactiveProperty<float> _velocidad = new SerializableReactiveProperty<float>(5f);

    [SerializeField]
    private SerializableReactiveProperty<bool> _invencible = new SerializableReactiveProperty<bool>(false);

    private DisposableBag _subs;

    void Start()
    {
        // Se usa exactamente igual que ReactiveProperty
        _nivel.Subscribe(n => Debug.Log($"Nivel: {n}"))
            .AddTo(ref _subs);

        _velocidad.Subscribe(v => Debug.Log($"Velocidad: {v}"))
            .AddTo(ref _subs);

        _invencible.Where(i => i == true)
            .Subscribe(_ => Debug.Log("Modo invencible activado!"))
            .AddTo(ref _subs);
    }

    void OnDestroy()
    {
        _subs.Dispose();
        _nivel.Dispose();
        _velocidad.Dispose();
        _invencible.Dispose();
    }
}
