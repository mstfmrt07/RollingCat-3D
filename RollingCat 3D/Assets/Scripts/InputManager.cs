using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject trailRenderer;

    Direction direction;
    Vector2 startPos, endPos;
    public float swipeThreshold = 0.2f;
    bool draggingStarted;

    private PlayerController player;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        player = PlayerController.Instance;

        draggingStarted = false;
        trailRenderer.SetActive(false);
        direction = Direction.None;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.gameStarted)
            GameManager.Instance.StartLevel();

        draggingStarted = true;
        trailRenderer.SetActive(true);
        startPos = eventData.pressPosition;

        UIManager.Instance.ActivateArrowsPanel(eventData.pressPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingStarted)
        {
            endPos = eventData.position;
            trailRenderer.transform.position = Camera.main.ScreenPointToRay(endPos).GetPoint(5f);

            Vector2 difference = endPos - startPos;

            if (difference.magnitude > swipeThreshold)
            {
                if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y)) // Do horizontal swipe
                {
                    direction = difference.x > 0 ? Direction.Right : Direction.Left;
                }
                else //do vertical swipe
                {
                    direction = difference.y > 0 ? Direction.Up : Direction.Down;
                }
            }
            else
            {
                direction = Direction.None;
            }

            UIManager.Instance.SetArrowDirection(direction);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingStarted)
        {
            if (direction != Direction.None)
            {
                StartCoroutine(player.RollToDirection(direction));
            }

            trailRenderer.SetActive(false);
            startPos = Vector2.zero;
            endPos = Vector2.zero;
            draggingStarted = false;

            UIManager.Instance.DeactivateArrowsPanel();
        }
    }
}

public enum Direction { Left, Up, Right, Down, None }