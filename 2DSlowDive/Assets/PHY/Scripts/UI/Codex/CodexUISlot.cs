using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UISlot을 상속받을까라는 생각을 왜 리팩토링할때 떠올린거지 근데 가능한가..?
/// 일단 보류
/// </summary>
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
        icon.color = new Color(1, 1, 1, 0.15f); // 흐린 배경 느낌

        qeustionText.gameObject.SetActive(true);
        numberText.text = "No.000";
        numberText.gameObject.SetActive(true);

    }
}