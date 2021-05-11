using UnityEngine;

public class FPS_Stabilizer : MonoBehaviour
{
    public static FPS_Stabilizer Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        Application.targetFrameRate = 30;
    }

}
