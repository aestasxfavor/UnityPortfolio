using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISlot : MonoBehaviour
{
    [SerializeField] private Image itemIcon;      // 아이콘 이미지
    [SerializeField] private TMP_Text countText;  // x2, x3 표시할 텍스트

    public bool IsEmpty => isEmpty;
    public FishType FishType => fishType;
    public Sprite CurrentSprite => itemIcon.sprite;

    private bool isEmpty = true;
    private FishType fishType;   // 이 슬롯에 들어있는 물고기 종류
    private int count = 0;

    private void Awake()
    {
        Clear();    // 시작할 땐 완전 빈 슬롯
    }

    // 새 물고기 들어올 때 (첫 입장)
    public void SetItem(Sprite icon, FishType type)
    {
        fishType = type;
        isEmpty = false;
        count = 1;

        if (itemIcon != null)
        {
            itemIcon.enabled = true;
            itemIcon.sprite = icon;
            Debug.Log($"[UISlot] {type} 슬롯 아이콘 적용됨 {(icon != null ? icon.name : "NULL")}"); 
        }

        var rt = itemIcon.rectTransform;
        Debug.Log($"[UISlot] {type} 슬롯 아이콘 적용됨 {(icon != null ? icon.name : "NULL")} / 크기={rt.sizeDelta} / 활성={itemIcon.enabled}");

        UpdateCountText();
    }

    // 같은 물고기 또 잡았을 때
    public void AddCount()
    {
        if (isEmpty) return;   // 안전장치

        count++;
        UpdateCountText();
    }

    private void UpdateCountText()
    {
        if (countText == null) return;

        if (count <= 1)
            countText.text = "";          // 1마리일 땐 숫자 숨김
        else
            countText.text = "" + count.ToString(); // x2, x3 ...
    }

    // 슬롯 비우기 (나중에 쓸 일 있으면 사용)
    public void Clear()
    {
        isEmpty = true;
        fishType = default;
        count = 0;

        if (itemIcon != null)
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;
        }

        if (countText != null)
            countText.text = "";
    }

    public void SetCount(int newCount)
    {
        count = newCount;
        isEmpty = count <= 0;
        UpdateCountText();
    }
}
