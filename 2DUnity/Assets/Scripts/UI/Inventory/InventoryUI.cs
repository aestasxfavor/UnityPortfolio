using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour, UIClosable
{
    private FishInventoryData sharedInventoryData;

    [Header("슬롯 부모 오브젝트")]
    [SerializeField] private GameObject inventoryPanelRoot;
    [SerializeField] private Transform quickbarParent;
    [SerializeField] private Transform inventoryParent;

    [Header("슬롯 프리팹")]
    [SerializeField] private GameObject slotPrefab;

    [Header("슬롯 개수")]
    [SerializeField] private InventoryConfigSO inventoryConfig;

    private readonly List<UISlot> quickbarSlots = new();
    private readonly List<UISlot> inventorySlots = new();

    private PlayerCtrls playerCtrls;
    private bool isInventoryOpen = false;
    private bool isSlotsCreated = false;

    // GameManager에서 공용 데이터 주입 시 바로 UI 갱신
    public void SetInventoryData(FishInventoryData data)
    {
        sharedInventoryData = data;

        // 슬롯이 아직 생성 전이면 Start에서 Refresh 할 거라 여기선 리턴
        if (!isSlotsCreated) return;

        RefreshUI();
    }

    private void Awake()
    {
        playerCtrls = new PlayerCtrls();
        playerCtrls.Player.Inventory.performed += OnInventoryInput; // 연결
        playerCtrls.Enable();
    }

    private void OnInventoryInput(InputAction.CallbackContext _)
    {
        ToggleInventory();
    }

    private void OnEnable()
    {
        SceneFlowManager.Instance?.Register(this);

        if (FishInventoryService.Instance != null)
            FishInventoryService.Instance.OnInventoryChanged += RefreshUI; // 이벤트 구독
    }

    private void OnDisable()
    {
        SceneFlowManager.Instance?.Unregister(this);

        if (FishInventoryService.Instance != null)
            FishInventoryService.Instance.OnInventoryChanged -= RefreshUI; // 이벤트 해제

        if (playerCtrls != null)
        {
            playerCtrls.Player.Inventory.performed -= OnInventoryInput; // 입력 해제
            playerCtrls.Disable();
        }
    }

    private void Start()
    {
        inventoryPanelRoot.SetActive(true);

        CreateSlots(quickbarParent, quickbarSlots, inventoryConfig.quickbarSize);
        CreateSlots(inventoryParent, inventorySlots, inventoryConfig.inventorySize);

        inventoryPanelRoot.SetActive(false);

        isSlotsCreated = true;

        if (sharedInventoryData == null && GameManager.Instance != null)
        {
            sharedInventoryData = GameManager.Instance.GetStorageInventoryData();
          //  Debug.Log($"[InventoryUI] Start에서 sharedInventoryData 자동 주입 완료{sharedInventoryData?.name}");
        }

        // 슬롯 생성 끝났으니 여기서 갱신
        RefreshUI();
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanelRoot.SetActive(isInventoryOpen);

        if (isInventoryOpen)
            RefreshUI();
    }

    private void CreateSlots(Transform parent, List<UISlot> list, int size)
    {
        if (slotPrefab == null || parent == null)
        {
          //  Debug.LogError("[InventoryUI] 슬롯 생성 실패 (prefab 또는 parent 없음)");
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

    public void RefreshUI()
    {

        if (!isSlotsCreated) return; // 슬롯 생성 전 호출 방어

        if (sharedInventoryData == null)
        {
           // Debug.LogWarning("[InventoryUI] Refresh 실패 - storageInventoryData 없음");
            return;
        }

        foreach (var s in quickbarSlots) s.Clear();
        foreach (var s in inventorySlots) s.Clear();

        int index = 0;

        foreach (var fish in sharedInventoryData.caughtFishList)
        {
            if (fish == null) continue;

            Sprite icon = FishInventoryService.Instance != null
            ? FishInventoryService.Instance.GetFishSprite(fish.fishType) : null;

            if (icon == null) continue;


            if (index < quickbarSlots.Count)
            {
                quickbarSlots[index].SetItem(icon, fish.fishType, fish.count);
            }
            else
            {
                int invenIdx = index - quickbarSlots.Count;
                if (invenIdx >= inventorySlots.Count) break;

                inventorySlots[invenIdx].SetItem(icon, fish.fishType, fish.count);
            }

            index++;
        }

       // Debug.Log($"[InventoryUI] UI 새로고침 완료 ({sharedInventoryData.caughtFishList.Count}종)");
    }

    public void Close()
    {
        isInventoryOpen = false;

        if(inventoryPanelRoot != null)
        {
            inventoryPanelRoot.SetActive(false);
        }   
        
        transform.root.gameObject.SetActive(false);
    }
}
