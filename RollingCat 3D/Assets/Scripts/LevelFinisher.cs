using System.Collections;
using UnityEngine;

public class LevelFinisher : MonoBehaviour
{
    public FinisherType finisherType;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (finisherType)
            {
                case FinisherType.Success:
                    StartCoroutine(CheckFacingDirection());
                    break;
                case FinisherType.Fail:
                    GameManager.Instance.GameOver();
                    break;
                default:
                    Debug.LogError("Finisher type has not been set");
                    break;
            }
        }
    }

    IEnumerator CheckFacingDirection()
    {
        while (PlayerController.Instance.isRolling)
        {
            yield return new WaitForEndOfFrame();
        }

        if (PlayerController.Instance.dominantAxis == DominantAxis.Y)
            GameManager.Instance.FinishLevel();
    }
}

public enum FinisherType { Success, Fail}