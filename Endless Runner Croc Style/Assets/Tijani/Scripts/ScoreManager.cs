using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    #region Singleton
    private static ScoreManager m_instance;
    public static ScoreManager Instance { get => m_instance; }

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
    private float _currentScore, _highScore;

    private string _highscoreKeyName = "Highscore Int";

    private void Update()
    {
        UpdateScore();
    }

    public void UpdateScore(float multiplier = 0)
    {
        if (multiplier > 0)
        {
            _currentScore += 10 * Time.deltaTime * multiplier;
        }
        else
        {
            _currentScore += 10 * Time.deltaTime;
        }

        if (_currentScore > PlayerPrefs.GetFloat(_highscoreKeyName))
        {
            PlayerPrefs.SetFloat(_highscoreKeyName, _currentScore);
            PlayerPrefs.Save();
        }

        _highScore = PlayerPrefs.GetFloat(_highscoreKeyName);
        UIManager.Instance.UpdateScoreText(_currentScore, _highScore);
    }

    public void DeleteHighscore()
    {
        PlayerPrefs.SetFloat(_highscoreKeyName, 0);
        UIManager.Instance.UpdateScoreText(_currentScore, _highScore);
        PlayerPrefs.Save();
    }
}
