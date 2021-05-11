using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public CanvasGroup arrowsPanel;
    public Image[] arrowImages;
    public Text levelText, startText;
    public Text collectableCountText;
    public GameObject waterPanel;

    public static UIManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        waterPanel.SetActive(false);
        arrowsPanel.alpha = 0f;
        arrowsPanel.transform.localScale = Vector3.zero;
    }

    public void ActivateArrowsPanel(Vector2 position)
    {
        arrowsPanel.DOFade(1f, 0.4f);
        arrowsPanel.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
        arrowsPanel.transform.position = position;
    }
    
    public void DeactivateArrowsPanel()
    {
        arrowsPanel.DOFade(0f, 0.4f);
        arrowsPanel.transform.DOScale(0f, 0.4f).SetEase(Ease.InBack);
    }

    public void SetArrowDirection(Direction _direction)
    {
        for (int i = 0; i < arrowImages.Length; i++)
        {
            if (i == (int)_direction)
            {
                arrowImages[i].color = Color.white;
            }
            else
            {
                Color color = arrowImages[i].color;
                color.a = 128f / 255f;
                arrowImages[i].color = color;
            }
        }
    }
}