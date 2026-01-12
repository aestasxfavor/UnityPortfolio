using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageUI : MonoBehaviour
{
    [Header("공용 인벤토리 데이터")]
    [SerializeField] private FishInventoryData sharedInventoryData;

    [Header("슬롯 관련 설정")]
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private List<FishType> slotFishOrder;
    [SerializeField] private int totalSlots = 27;

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

    // GameManager에서 데이터 주입받기
    public void SetInventoryData(FishInventoryData data)
    {
        sharedInventoryData = data;
        Debug.Log($"[StorageUI] storageInventoryData 주입 완료 ({data?.name})");
        LoadFishData();
    }

    // 슬롯 생성
    private void CreateSlots()
    {
        if(slotFishOrder == null)
        {
            slotFishOrder = new List<FishType>();

        }
        if (inventoryParent == null || slotPrefab == null)
        {
            Debug.LogError("[StorageUI] 슬롯 생성 실패 (부모나 프리팹이 비어 있음)");
            return;
        }

        for (int i = 0; i < totalSlots; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, inventoryParent);
            UISlot slot = newSlot.GetComponent<UISlot>();
            allSlots.Add(slot);
        }

        for (int i = 0; i < slotFishOrder.Count && i < allSlots.Count; i++)
        {
            slotMap[slotFishOrder[i]] = allSlots[i];
            Debug.Log($"[StorageUI] {slotFishOrder[i]} → {i + 1}번 슬롯 매핑 완료");
        }

        Debug.Log($"[StorageUI] 총 {slotMap.Count}/{totalSlots} 슬롯 매핑 완료");
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
        if (sharedInventoryData == null)
        {
            Debug.LogWarning("[StorageUI] sharedInventoryData가 비어 있음");
            return;
        }

        ClearSlots();

        Debug.Log($"[StorageUI] 불러오기 시작 ({sharedInventoryData.caughtFishList.Count}종)");

        foreach (var fish in sharedInventoryData.caughtFishList)
        {
            if (fish == null) continue;

            Sprite icon = fish.fishIcon;
            if (icon == null)
            {
                string path = $"Sprites/Fish/{fish.fishType}";
                Sprite[] sprites = Resources.LoadAll<Sprite>(path);
                icon = sprites.Length > 0 ? sprites[0] : Resources.Load<Sprite>(path);
            }

            if (slotMap.TryGetValue(fish.fishType, out UISlot slot))
            {
                slot.SetItem(icon, fish.fishType, fish.count);
                //slot.SetCount(fish.count);
                Debug.Log($"[StorageUI] {fish.fishType} → 지정 슬롯 표시");
            }
            else
            {
                UISlot emptySlot = allSlots.Find(s => s.IsEmpty && !slotMap.ContainsValue(s));
                if (emptySlot != null)
                {
                    emptySlot.SetItem(icon, fish.fishType, fish.count);
                   //emptySlot.SetCount(fish.count);
                    Debug.Log($"[StorageUI] {fish.fishType} → 여분 슬롯 표시");
                }
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryParent.GetComponent<RectTransform>());
        Debug.Log("[StorageUI] 보관함 UI 갱신 완료");
    }

    // 버튼에서 호출할 UI 토글 함수
    public void ToggleInventoryUI()
    {
        if (inventoryPanelRoot == null)
        {
            Debug.LogWarning("[StorageUI] inventoryPanelRoot 연결 안 됨");
            return;
        }

        bool newState = !inventoryPanelRoot.activeSelf;
        inventoryPanelRoot.SetActive(newState);

        if (newState)
        {
            LoadFishData();
            Debug.Log("[StorageUI] 보관함 열림");
        }
        else
        {
            Debug.Log("[StorageUI] 보관함 닫힘");
        }
    }
}
