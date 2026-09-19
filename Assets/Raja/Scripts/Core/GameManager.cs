using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }
        = GameState.MainMenu;

    private GameState previousState;

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

    private void Start()
    {
        SetState(GameState.MainMenu);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        SetState(GameState.DefensePlanning);
    }

    public void PauseGame()
    {
        // Hanya bisa pause saat gameplay berjalan
        if (CurrentState != GameState.Playing &&
            CurrentState != GameState.WaveBreak)
        {
            return;
        }

        previousState = CurrentState;

        Time.timeScale = 0f;
        SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused)
            return;

        Time.timeScale = 1f;
        SetState(previousState);
    }

    public void GameOver()
    {
        Time.timeScale = 1f;
        SetState(GameState.GameOver);
    }

    public void Victory()
    {
        Time.timeScale = 1f;
        SetState(GameState.Victory);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;

        Debug.Log($"Game State berubah menjadi: {CurrentState}");
    }
}