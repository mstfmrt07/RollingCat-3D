using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool gameStarted, gameOver, canStartGame;
    public int collectableCount;

    public Text collectableCountText;
    public static GameManager Instance;

    private Level currentLevel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        Initialize();
        CreateLevel(true);
    }

    void Initialize()
    {
        if (!PlayerPrefs.HasKey("CollectableCount"))
            PlayerPrefs.SetInt("CollectableCount", 0);

        collectableCount = PlayerPrefs.GetInt("CollectableCount");
        collectableCountText.text = collectableCount.ToString("00");
    }

    public void CreateLevel(bool isNewGame,  Level oldLevel = null)
    {
        gameStarted = false;
        gameOver = false;
        canStartGame = false;

        if (isNewGame)
        {
            canStartGame = true;
            currentLevel = Instantiate(LevelManager.Instance.levels[LevelManager.Instance.GetCurrentLevel()]);
            PlayerController.Instance.transform.position = currentLevel.spawnPoint.position;
        }
        else
        {
            Vector3 instantiatePos = new Vector3(PlayerController.Instance.transform.position.x, oldLevel.transform.position.y, PlayerController.Instance.transform.position.z) - (Vector3.up * 15f);

            currentLevel = Instantiate(LevelManager.Instance.levels[LevelManager.Instance.GetCurrentLevel()], instantiatePos, Quaternion.identity);
            Vector3 difference = new Vector3(currentLevel.transform.position.x - currentLevel.spawnPoint.position.x, 0f, currentLevel.transform.position.z - currentLevel.spawnPoint.position.z);
            currentLevel.transform.position += difference;
            Debug.Log(difference);
        }

        UIManager.Instance.levelText.text = "LEVEL " + (LevelManager.Instance.GetCurrentLevel() + 1);
    }

    public void StartLevel()
    {
        UIManager.Instance.startText.gameObject.SetActive(false);
        PlayerController.Instance.canMove = true;
        gameStarted = true;
        canStartGame = false;
    }

    public void FinishLevel()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.victoryClip);
        Level oldLevel = currentLevel;
        StartCoroutine(DeactivateLevel(currentLevel));
        currentLevel = null;
        PlayerController.Instance.canMove = false;
        LevelManager.Instance.NextLevel();
        CreateLevel(false, oldLevel);

    }

    IEnumerator DeactivateLevel(Level level)
    {
        Collider[] childColliders = level.gameObject.GetComponentsInChildren<Collider>();

        foreach (Collider collider in childColliders)
        {
            Destroy(collider);
            Debug.Log("Collider destroyed at "+ collider.name);
        }
        yield return new WaitForSeconds(2f);
        canStartGame = true;
        Destroy(level.gameObject);
        UIManager.Instance.startText.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.gameOverClip);
        gameOver = true;
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(SoundManager.Instance.gameOverClip.length);
        LevelManager.Instance.RestartLevel();
    }

    public void PickCollectable()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.collectClip);

        int collectableCount = PlayerPrefs.GetInt("CollectableCount") + 1;

        PlayerPrefs.SetInt("CollectableCount", collectableCount);
        collectableCountText.text = collectableCount.ToString("00");
    }
}
