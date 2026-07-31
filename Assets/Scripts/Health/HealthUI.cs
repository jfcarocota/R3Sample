using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth player;
    [SerializeField] private TMP_Text hpLabel;
    [SerializeField] private Image hpBar;

    private DisposableBag _disposables;

    void Start()
    {
        // Cada vez que HP cambia, actualiza el texto y la barra
        player.HP
            .Subscribe(hp =>
            {
                hpLabel.text = $"{hp} / {player.MaxHp}";
                hpBar.fillAmount = (float)hp / player.MaxHp;
            })
            .AddTo(ref _disposables);

        // Solo reacciona cuando el jugador muere (hp llega a 0)
        player.HP
            .Where(hp => hp <= 0)
            .Take(1) // solo una vez
            .Subscribe(_ => OnPlayerDied())
            .AddTo(ref _disposables);
    }

    void OnDestroy()
    {
        _disposables.Dispose();
    }

    private void OnPlayerDied()
    {
        hpLabel.text = "MUERTO";
        hpBar.fillAmount = 0f;
        Debug.Log("El jugador murió");
    }
}
