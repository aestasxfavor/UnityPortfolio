using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishToCoinSlot : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image frame;
    [SerializeField] private Image icon;

    [Header("Sprites")]
    [SerializeField] private Sprite normalFrameSprite;

    [Header("Texts")]
    [SerializeField] private TMP_Text countText;

    private FishType fishType;
    private bool hasItem;

    public void Set(Sprite sprite, int count, FishType type)
    {
        fishType = type;
        hasItem = true;

        if (frame != null && normalFrameSprite != null)
        {
            frame.sprite = normalFrameSprite;
            frame.color = Color.white;
        }

        icon.sprite = sprite;
        icon.enabled = true;
        icon.color = Color.white;

        countText.text = count.ToString();
        countText.enabled = true;
    }

    public void SetEmpty(Sprite emptyFrameSprite)
    {
        hasItem = false;
        fishType = default;

        if (frame != null && emptyFrameSprite != null)
        {
            frame.sprite = emptyFrameSprite;
            frame.color = Color.white;
        }

        icon.sprite = null;
        icon.enabled = false;

        countText.text = "";
        countText.enabled = false;
    }
}