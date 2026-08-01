using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lesson13_UI : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private Image hpBar;
    [SerializeField] private Image manaBar;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Button habilidadBtn;

    private ReactiveProperty<int> _hp    = new ReactiveProperty<int>(100);
    private ReactiveProperty<int> _mana  = new ReactiveProperty<int>(100);
    private ReactiveProperty<int> _score = new ReactiveProperty<int>(0);

    private DisposableBag _subs;

    void Start()
    {
        // HP: actualiza barra y texto juntos
        _hp.Subscribe(hp =>
        {
            hpBar.fillAmount = hp / 100f;
            hpText.text = $"HP: {hp}";
        }).AddTo(ref _subs);

        // Mana: solo actualiza la barra
        _mana.Subscribe(m => manaBar.fillAmount = m / 100f)
            .AddTo(ref _subs);

        // Score: transforma int a string antes de tocar la UI
        _score.Select(s => $"Score: {s:000}")
            .Subscribe(texto => scoreText.text = texto)
            .AddTo(ref _subs);

        // Botón activo solo si hay mana suficiente
        _mana.Select(m => m >= 30)
            .DistinctUntilChanged()
            .Subscribe(activo => habilidadBtn.interactable = activo)
            .AddTo(ref _subs);

        // Click del botón consume mana
        habilidadBtn.OnClickAsObservable()
            .Subscribe(_ =>
            {
                _mana.Value  -= 30;
                _score.Value += 50;
                Debug.Log("Habilidad usada!");
            }).AddTo(ref _subs);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) _hp.Value   = Mathf.Max(0, _hp.Value - 10);
        if (Input.GetKeyDown(KeyCode.H)) _hp.Value   = Mathf.Min(100, _hp.Value + 10);
        if (Input.GetKeyDown(KeyCode.M)) _mana.Value = Mathf.Min(100, _mana.Value + 30);
    }

    void OnDestroy()
    {
        _subs.Dispose();
        _hp.Dispose();
        _mana.Dispose();
        _score.Dispose();
    }
}
