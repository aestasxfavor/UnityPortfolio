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
    public void SetItem(Sprite icon, FishType type, int newCount)
    {
        fishType = type;
        isEmpty = false;

        if (itemIcon != null)
        {
            itemIcon.sprite = icon;
            itemIcon.enabled = icon != null;

#if UNITY_EDITOR
            Debug.Log($"[UISlot] {type} 아이콘 적용 {(icon != null ? icon.name : "NULL")}");
#endif
        }

        count = Mathf.Max(1, newCount);
        UpdateCountText();
    }

    private void UpdateCountText()
    {
        //if (countText == null) return;

        //if (count <= 1)
        //    countText.text = "";          // 1마리일 땐 숫자 숨김
        //else
        //    countText.text = count.ToString(); // x2, x3 ...

        if (countText == null) return;

        if (count <= 1)
        {
            countText.enabled = false;
        }
        else
        {
            countText.enabled = true;
            countText.text = count.ToString();
        }
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
        {
            countText.text = "";
            countText.enabled = false;
        }
    }
}
