using TMPro;
using UnityEngine;

public class CodexTooltipUI : MonoBehaviour
{
    public static CodexTooltipUI Instance { get; private set; }

    [Header("Root")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform tooltipRect;

    [Header("UI")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    [Header("Position")]
    [SerializeField] private Vector2 offset = new Vector2(24f, 0f);

    private void Awake()
    {
        //Debug.Log("[CodexTooltipUI] Awake 호출됨");

        Instance = this;

        if (canvasGroup == null)
            //Debug.LogWarning("[CodexTooltipUI] CanvasGroup 연결 안 됨");

        if (tooltipRect == null)
            //Debug.LogWarning("[CodexTooltipUI] Tooltip Rect 연결 안 됨");

        if (nameText == null)
            //Debug.LogWarning("[CodexTooltipUI] Name Text 연결 안 됨");

        if (descriptionText == null)
           //Debug.LogWarning("[CodexTooltipUI] Description Text 연결 안 됨");

        Hide();
    }

    public void Show(FishCodexSO codexData, RectTransform slotRect)
    {
        //Debug.Log("[CodexTooltipUI] Show 호출됨");

        if (codexData == null)
        {
            //Debug.LogWarning("[CodexTooltipUI] codexData가 null이라 툴팁 표시 중단");
            return;
        }

        if (slotRect == null)
        {
            //Debug.LogWarning("[CodexTooltipUI] slotRect가 null이라 툴팁 표시 중단");
            return;
        }

       // Debug.Log($"[CodexTooltipUI] 표시할 이름: {codexData.fishName}");
       // Debug.Log($"[CodexTooltipUI] 표시할 설명: {codexData.description}");

        if (nameText != null)
        {
            nameText.text = codexData.fishName;
            //Debug.Log("[CodexTooltipUI] nameText 적용 완료");
        }
        else
        {
           // Debug.LogWarning("[CodexTooltipUI] nameText가 null이라 이름 적용 불가");
        }

        if (descriptionText != null)
        {
            descriptionText.text = codexData.description;
            //Debug.Log("[CodexTooltipUI] descriptionText 적용 완료");
        }
        else
        {
           // Debug.LogWarning("[CodexTooltipUI] descriptionText가 null이라 설명 적용 불가");
        }

        SetPosition(slotRect);
        ShowPanel();
    }

    public void Hide()
    {
      //  Debug.Log("[CodexTooltipUI] Hide 호출됨");

        if (canvasGroup == null)
        {
           // Debug.LogWarning("[CodexTooltipUI] CanvasGroup이 null이라 Hide 처리 불가");
            return;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

       // Debug.Log("[CodexTooltipUI] 툴팁 숨김 처리 완료");
    }

    private void ShowPanel()
    {
       // Debug.Log("[CodexTooltipUI] ShowPanel 호출됨");

        if (canvasGroup == null)
        {
            //Debug.LogWarning("[CodexTooltipUI] CanvasGroup이 null이라 ShowPanel 처리 불가");
            return;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        //Debug.Log("[CodexTooltipUI] 툴팁 표시 처리 완료");
    }

    private void SetPosition(RectTransform slotRect)
    {
       // Debug.Log("[CodexTooltipUI] SetPosition 호출됨");

        if (tooltipRect == null)
        {
            //Debug.LogWarning("[CodexTooltipUI] Tooltip Rect가 null이라 위치 이동 불가");
            return;
        }

        Vector3[] corners = new Vector3[4];
        slotRect.GetWorldCorners(corners);

        Vector3 leftBottom = corners[0];
        Vector3 leftTop = corners[1];
        Vector3 rightTop = corners[2];
        Vector3 rightBottom = corners[3];

       // Debug.Log($"[CodexTooltipUI] 슬롯 좌하단: {leftBottom}");
       // Debug.Log($"[CodexTooltipUI] 슬롯 좌상단: {leftTop}");
       // Debug.Log($"[CodexTooltipUI] 슬롯 우상단: {rightTop}");
       // Debug.Log($"[CodexTooltipUI] 슬롯 우하단: {rightBottom}");

        Vector3 rightCenter = (rightTop + rightBottom) * 0.5f;
        tooltipRect.position = rightCenter + new Vector3(offset.x, offset.y, 0f);

        //Debug.Log($"[CodexTooltipUI] 툴팁 최종 위치: {tooltipRect.position}");
    }

    public void ShowTest(RectTransform slotRect)
    {
       //Debug.Log("[CodexTooltipUI] ShowTest 호출됨");

        if (slotRect == null)
        {
            //Debug.LogWarning("[CodexTooltipUI] slotRect null이라 테스트 툴팁 표시 중단");
            return;
        }

        if (nameText != null)
            nameText.text = "툴팁 테스트";

        if (descriptionText != null)
            descriptionText.text = "도감 슬롯 호버 감지 확인 중";

        SetPosition(slotRect);
        ShowPanel();
    }
}