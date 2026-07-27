using R3;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHp = 100;

    private ReactiveProperty<int> _hp;

    public ReadOnlyReactiveProperty<int> HP => _hp;
    public int MaxHp => maxHp;

    void Awake()
    {
        _hp = new ReactiveProperty<int>(maxHp);
    }

    public void TakeDamage(int damage)
    {
        _hp.Value = Mathf.Max(0, _hp.Value - damage);
    }

    public void Heal(int amount)
    {
        _hp.Value = Mathf.Min(maxHp, _hp.Value + amount);
    }

    void OnDestroy()
    {
        _hp.Dispose();
    }
}
