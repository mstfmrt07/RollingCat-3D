using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Level[] levels;
    public static LevelManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
            Destroy(gameObject);

        LevelControl();
    }

    public void LevelControl()
    {
        if (!PlayerPrefs.HasKey("CurrentLevel"))
            PlayerPrefs.SetInt("CurrentLevel", 0);

        if (PlayerPrefs.GetInt("CurrentLevel") >= levels.Length)
            PlayerPrefs.SetInt("CurrentLevel", 0);
    }

    public void NextLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);
        LevelControl();
    }

    public void RestartLevel()
    {
        DG.Tweening.DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt("CurrentLevel");
    }

    public void ResetProgress(int levelIndex)
    {
        PlayerPrefs.SetInt("CurrentLevel", levelIndex);
        LevelControl();
        DG.Tweening.DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
