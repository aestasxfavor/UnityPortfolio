using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TooltipPlacement
{
    Above,
    Below
}

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    [Header("Tooltip UI")]
    [SerializeField] private RectTransform tooltipRoot;
    [SerializeField] private TMP_Text tooltipText;

    [Header("Tooltip Image")]
    [SerializeField] private Image tooltipBoxImage;
    [SerializeField] private Sprite aboveSprite;
    [SerializeField] private Sprite belowSprite;

    [Header("Tooltip Text Position")]
    [SerializeField] private RectTransform tooltipTextRect;
    [SerializeField] private Vector2 aboveTextPosition = new Vector2(0f, 6f);
    [SerializeField] private Vector2 belowTextPosition = new Vector2(0f, -6f);

    [Header("Tooltip Canvas")]
    [SerializeField] private Canvas tooltipCanvas;

    [Header("Position")]
    [SerializeField] private float aboveOffsetY = 45f;
    [SerializeField] private float belowOffsetY = -45f;
    [SerializeField] private float screenPadding = 5f;

    private RectTransform currentTarget;
    private TooltipPlacement currentPlacement;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (tooltipCanvas == null)
        {
            tooltipCanvas = GetComponentInParent<Canvas>();
        }

        HideTooltip();
    }

    private void LateUpdate()
    {
        if (currentTarget == null || tooltipRoot == null || !tooltipRoot.gameObject.activeSelf)
        {
            return;
        }

        SetPositionByTarget(currentTarget, currentPlacement);
    }

    public void ShowTooltip(string text, RectTransform target, TooltipPlacement placement)
    {
        if (tooltipRoot == null || tooltipText == null || target == null)
        {
            return;
        }

        currentTarget = target;
        currentPlacement = placement;

        tooltipText.text = text;

        if (tooltipBoxImage != null)
        {
            tooltipBoxImage.sprite = placement == TooltipPlacement.Above ? aboveSprite : belowSprite;
        }

        if (tooltipTextRect != null)
        {
            tooltipTextRect.anchoredPosition = placement == TooltipPlacement.Above
                ? aboveTextPosition
                : belowTextPosition;
        }

        tooltipRoot.gameObject.SetActive(true);
        tooltipRoot.SetAsLastSibling();

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRoot);

        SetPositionByTarget(target, placement);
    }

    public void HideTooltip()
    {
        currentTarget = null;

        if (tooltipRoot != null)
        {
            tooltipRoot.gameObject.SetActive(false);
        }
    }

    private void SetPositionByTarget(RectTransform target, TooltipPlacement placement)
    {
        RectTransform tooltipParent = tooltipRoot.parent as RectTransform;

        if (tooltipParent == null)
        {
            return;
        }

        Vector3[] corners = new Vector3[4];
        target.GetWorldCorners(corners);

        Vector3 targetWorldPosition;

        if (placement == TooltipPlacement.Above)
        {
            targetWorldPosition = (corners[1] + corners[2]) * 0.5f;
        }
        else
        {
            targetWorldPosition = (corners[0] + corners[3]) * 0.5f;
        }

        Camera targetCamera = GetCanvasCamera(target);
        Camera tooltipCamera = GetTooltipCamera();

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(
            targetCamera,
            targetWorldPosition
        );

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tooltipParent,
                screenPoint,
                tooltipCamera,
                out Vector2 localPoint))
        {
            return;
        }

        float offsetY = placement == TooltipPlacement.Above ? aboveOffsetY : belowOffsetY;

        Vector2 finalPosition = localPoint + new Vector2(0f, offsetY);
        tooltipRoot.anchoredPosition = ClampToParent(finalPosition, tooltipParent);
    }

    private Camera GetCanvasCamera(RectTransform rect)
    {
        Canvas targetCanvas = rect.GetComponentInParent<Canvas>();

        if (targetCanvas == null)
        {
            return null;
        }

        if (targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            return null;
        }

        return targetCanvas.worldCamera;
    }

    private Camera GetTooltipCamera()
    {
        if (tooltipCanvas == null)
        {
            return null;
        }

        if (tooltipCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            return null;
        }

        return tooltipCanvas.worldCamera;
    }

    private Vector2 ClampToParent(Vector2 position, RectTransform parent)
    {
        Vector2 tooltipSize = tooltipRoot.rect.size;

        float minX = parent.rect.xMin + tooltipSize.x * 0.5f + screenPadding;
        float maxX = parent.rect.xMax - tooltipSize.x * 0.5f - screenPadding;

        float minY = parent.rect.yMin + tooltipSize.y * 0.5f + screenPadding;
        float maxY = parent.rect.yMax - tooltipSize.y * 0.5f - screenPadding;

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);

        return position;
    }
}