using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MSingleton<InputManager>, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject trailRenderer;
    public float swipeThreshold = 100f;

    public Action<Direction> OnSwipeDetected;

    private Direction direction;
    private Vector2 startPos;
    private Vector2 endPos;
    private bool draggingStarted;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        draggingStarted = false;
        trailRenderer.SetActive(false);
        direction = Direction.None;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggingStarted = true;
        trailRenderer.SetActive(true);
        startPos = eventData.pressPosition;

        if (!GameManager.Instance.gameStarted && GameManager.Instance.canStartGame)
            GameManager.Instance.StartLevel();
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
                OnSwipeDetected?.Invoke(direction);
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