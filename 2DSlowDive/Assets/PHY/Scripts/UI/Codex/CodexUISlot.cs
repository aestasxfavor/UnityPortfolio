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

    private void Awake()
    {
        if (hoverOverlayImage != null)
            hoverOverlayImage.gameObject.SetActive(false);
    }

    public void SetDiscovered(Sprite fishSprite, int number)
    {
        isDiscovered = true;
        codexNumber = number;

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
        if (hoverOverlayImage != null)
            hoverOverlayImage.gameObject.SetActive(true);

        if (isDiscovered)
        {
            // ¹ß°ßµÈ ¹°°í±â ÅøÆÁ Ç¥½Ã
        }
        else
        {
            // ¹Ì¹ß°ß »ý¹° ÅøÆÁ Ç¥½Ã
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverOverlayImage != null)
            hoverOverlayImage.gameObject.SetActive(false);

        // ÅøÆÁ ¼û±è
    }
}