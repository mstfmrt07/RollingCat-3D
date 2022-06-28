using UnityEngine;

public class FPS_Stabilizer : MSingleton<FPS_Stabilizer>
{
    void Awake()
    {
        Application.targetFrameRate = 30;
    }

}
