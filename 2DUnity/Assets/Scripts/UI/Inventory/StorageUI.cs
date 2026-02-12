using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageUI : MonoBehaviour
{
    [Header("공용 인벤토리 데이터")]
    //private FishInventoryData sharedInventoryData;

    [Header("슬롯 관련 설정")]
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private GameObject slotPrefab;

    [SerializeField] private StorageConfigSO storageConfig;

    [Header("UI 활성화 루트 (Inventory Bar 연결)")]
    [SerializeField] private GameObject inventoryPanelRoot;

    private readonly Dictionary<FishType, UISlot> slotMap = new();
    private readonly List<UISlot> allSlots = new();

    /// <summary>
    /// 보관함은 Awake에서 슬롯을 생성하고 GameManager로부터 받은 데이터로 UI를 즉시 갱신
    /// 매핑한 각 슬롯이 FishType 에 맞는 스프라이트, 개수 표시
    /// </summary>

    private void Awake()
    {
        CreateSlots();      // 빈슬롯 생성
        if (inventoryPanelRoot != null)
            inventoryPanelRoot.SetActive(false); // 시작 시 닫힘 상태
    }

    private void OnEnable()
    {
        if(FishInventoryService.Instance != null)
        {
            FishInventoryService.Instance.OnInventoryChanged += OnInventoryChanged;
        }
    }

    private void OnDisable()
    {
        if(FishInventoryService.Instance != null)
        {
            FishInventoryService.Instance.OnInventoryChanged -= OnInventoryChanged;
        }
    }

    private void OnInventoryChanged()
    {
        if(inventoryPanelRoot != null && inventoryPanelRoot.activeSelf)
        {
            LoadFishData();
        }
    }

    // 슬롯 생성
    private void CreateSlots()
    {
        if (storageConfig == null)
        {
            Debug.LogError("[StorageUI] storageConfig가 비어 있음");
            return;
        }

        if (storageConfig.slotFishOrder == null)
        {
            storageConfig.slotFishOrder = new List<FishType>();

        }
        if (inventoryParent == null || slotPrefab == null)
        {
           // Debug.LogError("[StorageUI] 슬롯 생성 실패 (부모나 프리팹이 비어 있음)");
            return;
        }

        for (int i = 0; i < storageConfig.totalSlots; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, inventoryParent);
            UISlot slot = newSlot.GetComponent<UISlot>();
            allSlots.Add(slot);
        }

        for (int i = 0; i < storageConfig.slotFishOrder.Count && i < allSlots.Count; i++)
        {
            slotMap[storageConfig.slotFishOrder[i]] = allSlots[i];
           // Debug.Log($"[StorageUI] {storageConfig.slotFishOrder[i]} → {i + 1}번 슬롯 매핑 완료");
        }

        Debug.Log($"[StorageUI] 총 {slotMap.Count}/{storageConfig.totalSlots} 슬롯 매핑 완료");
    }

    // 슬롯 초기화
    private void ClearSlots()
    {
        foreach (var slot in allSlots)
            slot.Clear();
    }

    // 인벤토리 데이터 로드
    public void LoadFishData()
    {
        ClearSlots();


        var service = FishInventoryService.Instance;
        if (service == null) return;

        var fishList = service.GetFishViewList();
        if (fishList == null) return;

        foreach (var fish in fishList)
        {
            Sprite icon = service.GetFishSprite(fish.fishType);
            if (icon == null) continue;

            if(slotMap.TryGetValue(fish.fishType, out UISlot slot))
            {
                slot.SetItem(icon, fish.fishType, fish.count);
            }
            else
            {
                UISlot emptySlot = null;

                for (int i = 0; i < allSlots.Count; i++)
                {
                    var s = allSlots[i];
                    if (!s.IsEmpty) continue;

                    if (slotMap.ContainsValue(s)) continue;

                    emptySlot = s;
                    break;

                }

                if (emptySlot != null)
                {
                    emptySlot.SetItem(icon, fish.fishType, fish.count);
                }
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryParent.GetComponent<RectTransform>());
       // Debug.Log("[StorageUI] 보관함 UI 갱신 완료");
    }

    public void Open()
    {
        Debug.Log("보관함 열림");
        if (inventoryPanelRoot == null) return;

        inventoryPanelRoot.SetActive(true);
        LoadFishData();

    }
}
