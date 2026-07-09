using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CodexUISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Images")]
    [SerializeField] private Image frameImage;
    [SerializeField] private Image unknownImage;
    [SerializeField] private Image hoverOverlayImage;
    [SerializeField] private Image fishIcon;

    [Header("Texts")]
    [SerializeField] private TMP_Text numberText;

    private bool isDiscovered;
    private int codexNumber;

    private FishCodexSO fishCodexData;

    private void Awake()
    {
        if (hoverOverlayImage != null)
            hoverOverlayImage.gameObject.SetActive(false);
    }

    public void SetDiscovered(Sprite fishSprite, int number, FishCodexSO codexData)
    {
        isDiscovered = true;
        codexNumber = number;
        fishCodexData = codexData;

        if (frameImage != null)
            frameImage.gameObject.SetActive(true);

        if (unknownImage != null)
            unknownImage.gameObject.SetActive(false);

        if (fishIcon != null)
        {
            fishIcon.gameObject.SetActive(true);
            fishIcon.sprite = fishSprite;
            fishIcon.color = Color.white;
        }

        if (numberText != null)
        {
            numberText.gameObject.SetActive(true);
            numberText.text = $"No.{number:000}";
        }
    }

    public void SetUndiscovered(Sprite unknownSprite, int number)
    {
        isDiscovered = false;
        codexNumber = number;
        fishCodexData = null;

        if (frameImage != null)
            frameImage.gameObject.SetActive(false);

        if (unknownImage != null)
        {
            unknownImage.gameObject.SetActive(true);
            unknownImage.sprite = unknownSprite;
            unknownImage.color = Color.white;
        }

        if (fishIcon != null)
            fishIcon.gameObject.SetActive(false);

        if (numberText != null)
        {
            numberText.gameObject.SetActive(true);
            numberText.text = $"No.{number:000}";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("[CodexUISlot] Pointer Enter È£ÃâµÊ");

        if (hoverOverlayImage != null)
        {
            hoverOverlayImage.gameObject.SetActive(true);
        }

        if (CodexTooltipUI.Instance == null)
        {
            //Debug.LogWarning("[CodexUISlot] CodexTooltipUI.Instance ¾øÀ½");
            return;
        }

        // ¹ß°ßµÈ ¹°°í±â ÅøÆÁ Ç¥½Ã
        if (!isDiscovered || fishCodexData == null) return;
        
        RectTransform slotRect = transform as RectTransform;
        CodexTooltipUI.Instance.Show(fishCodexData, slotRect);

    }

    public void OnPointerExit(PointerEventData eventData)
    {

        Debug.Log("[CodexUISlot] Pointer Exit È£ÃâµÊ");

        if (hoverOverlayImage != null)
            hoverOverlayImage.gameObject.SetActive(false);

        // ÅøÆÁ ¼û±è
        if(CodexTooltipUI.Instance != null)
            CodexTooltipUI.Instance.Hide();
    }
}