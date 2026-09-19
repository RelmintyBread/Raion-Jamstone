using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public int CurrentLevel { get; private set; } = 1;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(string sceneName, int levelNumber)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("Nama scene tidak boleh kosong!");
            return;
        }

        CurrentLevel = levelNumber;

        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);
    }

    public void StartLevel()
    {
        GameManager.Instance.SetState(GameState.Playing);

        Debug.Log($"Level {CurrentLevel} dimulai!");
    }

    public void CompleteLevel()
    {
        GameManager.Instance.Victory();

        Debug.Log($"Level {CurrentLevel} selesai!");
    }

    public void FailLevel()
    {
        GameManager.Instance.GameOver();

        Debug.Log($"Level {CurrentLevel} gagal!");
    }
}