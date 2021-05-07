using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Interactable : MonoBehaviour
{
    public InteractableType interactableType;
    [SerializeField] private Transform bridgeTransform;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (interactableType)
            {
                case InteractableType.LevelSuccess:
                    StartCoroutine(CheckFacingDirection());
                    break;
                case InteractableType.LevelFail:
                    GameManager.Instance.GameOver();
                    break;
                case InteractableType.ToggleBridge:
                    StartCoroutine(ToggleBridge(bridgeTransform, !bridgeTransform.gameObject.activeSelf)); // toggle the reverse state
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

    IEnumerator ToggleBridge(Transform bridge, bool toggleState)
    {
        bool willSetAfterToggle = false;
        if (!bridgeTransform.gameObject.activeSelf)
            bridgeTransform.gameObject.SetActive(true);
        else
            willSetAfterToggle = true;

        foreach (Transform block in bridge)
        {
            block.DOScale(toggleState ? 1f : 0f, 0.4f).SetEase(toggleState ? Ease.OutBack : Ease.InBack);
            yield return new WaitForSeconds(0.2f);
        }

        if (willSetAfterToggle)
        {
            yield return new WaitForSeconds(0.3f);
            bridgeTransform.gameObject.SetActive(false);
        }
    }
}

public enum InteractableType { LevelSuccess, LevelFail, ToggleBridge}