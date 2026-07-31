using UnityEngine;
using R3;
using TMPro;
public class ScoreSystem : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject messageText;

    private ReactiveProperty<int> _score = new ReactiveProperty<int>(0);
    private DisposableBag _subs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _score.Select(s => $"Score: {s}")
            .Subscribe(text => scoreText.text = text)
            .AddTo(ref _subs);

        _score.Where(s => s >= 100)
            .Take(1)
            .Subscribe(_ => messageText.SetActive(true))
            .AddTo(ref _subs);

    }

    private void AddPoints(int points)
    {
        _score.Value += points;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            AddPoints(10);
        }
    }

    void OnDestroy()
    {
        _score.Dispose();
        _subs.Dispose();
    }
}
