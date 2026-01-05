using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, UIClosable
{
    [SerializeField] public FishInventoryData sharedInventoryData;

    [Header("슬롯 부모 오브젝트")]
    [SerializeField] private GameObject inventoryPanelRoot; // 전체 인벤토리 패널
    [SerializeField] private Transform quickbarParent;      // 퀵슬롯 부모
    [SerializeField] private Transform inventoryParent;     // 일반 인벤토리 부모

    [Header("슬롯 프리팹")]
    [SerializeField] private GameObject slotPrefab;

    [Header("슬롯 개수")]
    [SerializeField] private int quickbarSize = 9;
    [SerializeField] private int inventorySize = 27;

    private List<UISlot> quickbarSlots = new();
    private List<UISlot> inventorySlots = new();

    private PlayerCtrls playerCtrls;
    private bool isInventoryOpen = false;

    // GameManager에서 공용 데이터 주입 시 바로 UI 갱신
    public void SetInventoryData(FishInventoryData data)
    {
        sharedInventoryData = data;
        Debug.Log($"[InventoryUI] fishInventoryData 주입 완료 ({data?.name})");
        RefreshUI();
    }

    private void Awake()
    {
        playerCtrls = new PlayerCtrls();
        playerCtrls.Player.Inventory.performed += _ => ToggleInventory();
        playerCtrls.Enable();
    }

    private void OnDisable()
    {
        if (playerCtrls != null)
        {
            playerCtrls.Player.Inventory.performed -= _ => ToggleInventory();
            playerCtrls.Disable();
        }
    }

    private void Start()
    {
        inventoryPanelRoot.SetActive(true);
        CreateSlots(quickbarParent, quickbarSlots, quickbarSize);
        CreateSlots(inventoryParent, inventorySlots, inventorySize);
        inventoryPanelRoot.SetActive(false);
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanelRoot.SetActive(isInventoryOpen);
    }

    private void CreateSlots(Transform parent, List<UISlot> list, int size)
    {
        if (slotPrefab == null || parent == null)
        {
            Debug.LogError("[InventoryUI] 슬롯 생성 실패 (prefab 또는 parent 없음)");
            return;
        }

        for (int i = 0; i < size; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, parent);
            UISlot slot = newSlot.GetComponent<UISlot>();
            list.Add(slot);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetComponent<RectTransform>());
    }

    // 물고기 추가 (UI + 데이터 + JSON 통합)
    public void AddItemToUI(FishType fish, Sprite fishIcon)
    {
        if (fishIcon == null)
        {
            Debug.LogWarning($"[InventoryUI] {fish} 아이콘이 null");
            return;
        }

        // 데이터에 반영
        sharedInventoryData?.AddFish(fish, fishIcon);
        //CaughtFishManager.AddFish(fish.ToString());

        // 퀵바 먼저 채우기
        foreach (UISlot slot in quickbarSlots)
        {
            if (!slot.IsEmpty && slot.FishType == fish)
            {
                slot.AddCount();
                Debug.Log($"[InventoryUI] {fish} → 퀵바 수량 증가");
                return;
            }
        }

        foreach (UISlot slot in quickbarSlots)
        {
            if (slot.IsEmpty)
            {
                slot.SetItem(fishIcon, fish);
                Debug.Log($"[InventoryUI] {fish} → 퀵바 새 슬롯 추가");
                return;
            }
        }

        // 인벤토리 슬롯 채우기
        foreach (UISlot slot in inventorySlots)
        {
            if (!slot.IsEmpty && slot.FishType == fish)
            {
                slot.AddCount();
                Debug.Log($"[InventoryUI] {fish} → 인벤토리 수량 증가");
                return;
            }
        }

        foreach (UISlot slot in inventorySlots)
        {
            if (slot.IsEmpty)
            {
                slot.SetItem(fishIcon, fish);
                Debug.Log($"[InventoryUI] {fish} → 인벤토리 새 슬롯 추가");
                return;
            }
        }

        Debug.LogWarning("[InventoryUI] 인벤토리가 가득 찼습니다");
    }

    // JSON이나 ScriptableObject 데이터 기준으로 UI 리셋 후 다시 채움
    public void RefreshUI()
    {
        if (sharedInventoryData == null)
        {
            Debug.LogWarning("[InventoryUI] Refresh 실패 - fishInventoryData 없음");
            return;
        }

        // 모든 슬롯 초기화
        foreach (var s in quickbarSlots) s.Clear();
        foreach (var s in inventorySlots) s.Clear();

        // 데이터 기반으로 다시 채움
        foreach (var fish in sharedInventoryData.caughtFishList)
        {
            Sprite icon = fish.fishIcon ?? Resources.Load<Sprite>($"Sprites/Fish/{fish.fishType}");
            AddItemToUI(fish.fishType, icon);
        }

        Debug.Log($"[InventoryUI] UI 새로고침 완료 ({sharedInventoryData.caughtFishList.Count}종)");
    }

    public void Close()
    {
        transform.root.gameObject.SetActive(false);
    }
}
