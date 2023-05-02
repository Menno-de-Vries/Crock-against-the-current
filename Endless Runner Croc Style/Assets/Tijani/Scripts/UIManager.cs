using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager m_instance;
    public static UIManager Instance { get => m_instance; }

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else if (m_instance != this)
            Destroy(this);
    }
    #endregion Singleton

    [SerializeField]
    private TextMeshProUGUI _currentScoreText;
    [SerializeField]
    private TextMeshProUGUI _highScoreText;
    [SerializeField]
    private TextMeshProUGUI _monkeyCount;
    [SerializeField]
    private TextMeshProUGUI _currentHealth;

    private void Start()
    {
        UpdateMonkeyCount();
        UpdateHealth();
    }

    public void UpdateScoreText(float currentScore, float highscore)
    {
        _currentScoreText.text = "Current Score: " + currentScore.ToString("000000");
        _highScoreText.text = "Highscore: " + highscore.ToString("000000");
    }

    public void UpdateHealth()
    {
        _currentHealth.text = $"Lives: {GameManager.Instance.PlayerHealth.CurrentHealth}";
    }

    public void UpdateMonkeyCount()
    {
        _monkeyCount.text = $"Monkeys {GameManager.Instance.MonkeyCount} / {GameManager.Instance.MaxMonkeyCount}";
    }
}