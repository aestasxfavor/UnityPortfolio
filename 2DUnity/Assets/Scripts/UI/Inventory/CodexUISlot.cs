using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodexUISlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private TMP_Text qeustionText;

    public void SetDiscovered(Sprite sprite, int number)
    {
        icon.enabled = true;
        icon.sprite = sprite;
        icon.color = Color.white;

        qeustionText.gameObject.SetActive(false);
        numberText.text = $"No.{number}";
    }

    public void SetUndiscovered(Sprite unknownIcon, int number)
    {
        icon.enabled = false;
        icon.color = new Color(1, 1, 1, 0.15f); // »Â∏∞ πË∞Ê ¥¿≥¶

        qeustionText.gameObject.SetActive(true);
        numberText.text = "No.000";
        numberText.gameObject.SetActive(true);

    }
}