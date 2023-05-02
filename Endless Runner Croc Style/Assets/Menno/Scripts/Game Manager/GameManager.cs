using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

// Paths class gemaakt door tijani
[System.Serializable]
public class Paths
{
    public float PathPositionX;
    public bool IsPathFull;
}

// Gedaan door menno de code hier onder
public class GameManager : MonoBehaviour
{
    #region Static Instance and Lane Settings

    private static GameManager m_instance;
    public static GameManager Instance { get => m_instance; }

    [Tooltip("The x pos number for every lane")]
    public Paths[] Paths = new Paths[3];

    #endregion

    #region Monkey Settings

    [Header("Monkey Collectible Settings")]
    [Tooltip("How many monkeys you ate")]
    public int MonkeyCount;

    [Tooltip("The maximum of monkeys to gather to get a extra live")]
    public int MaxMonkeyCount = 20;

    #endregion

    #region Player Settings

    [Header("Player Settings")]
    public Health PlayerHealth;

    #endregion

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else if (m_instance != this)
            Destroy(this);
    }

    private void Start()
    {
        if (FindObjectOfType<PlayerController>() != null)
        {
            // Stores the health script of the player in this var
            PlayerHealth = FindObjectOfType<PlayerController>().GetComponent<Health>();
        }
    }

    public void MainHub()
    {
        SceneManager.LoadScene("Start");
    }

    public void StartGame()
    {
        SoundManager.Instance.Stop("MainMenu");
        SoundManager.Instance.Play("GameMusic");
        SceneManager.LoadScene("Game");
    }

    public void LoadInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Game Over");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
}
