using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameStarted;
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        InitializeLevel();
    }

    public void InitializeLevel()
    {
        gameStarted = false;
        Level currentLevel = Instantiate(LevelManager.Instance.levels[LevelManager.Instance.GetCurrentLevel()]);
        PlayerController.Instance.transform.position = currentLevel.spawnPoint.position;
        UIManager.Instance.levelText.text = "LEVEL " + (LevelManager.Instance.GetCurrentLevel() + 1);
    }

    public void StartLevel()
    {
        UIManager.Instance.startText.gameObject.SetActive(false);
        PlayerController.Instance.canMove = true;
        gameStarted = true;
    }

    public void FinishLevel()
    {
        LevelManager.Instance.NextLevel();
    }

    public void GameOver()
    {
        LevelManager.Instance.RestartLevel();
    }
}
