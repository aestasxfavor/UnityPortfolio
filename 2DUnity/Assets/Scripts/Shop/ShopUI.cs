using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Tab Buttons")]
    [SerializeField] private Button coinTabButton;
    [SerializeField] private Button shopTabButton;

    [Header("Tab Roots")]
    [SerializeField] private GameObject coinTabRoot;
    [SerializeField] private GameObject shopTabRoot;

    [Header("Coin Exchange")]
    [SerializeField] private Transform coinGridRoot;
    [SerializeField] private GameObject fishToCoinSlotPrefab;
    [SerializeField] private Sprite emptySprite;

    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI harpoonCoinText;
    [SerializeField] private PopupManager popupManager;

    private const int RequiredCoin = 1500;

    private void Awake()
    {
        coinTabButton.onClick.AddListener(OpenCoinTab);
        shopTabButton.onClick.AddListener(OpenShopTab);
    }

    private void Start()
    {
        UpdateCoinUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CoinService.Instance.AddCoin(5000);
            UpdateCoinUI();

            Debug.Log("테스트 코인 지급 완료");
        }
    }

    // 상점 열릴 때 기본 상태
    public void Open()
    {
        OpenCoinTab();

    }

    public void OpenCoinTab()
    {
        Debug.Log("코인탭 전환");
        coinTabRoot.SetActive(true);
        shopTabRoot.SetActive(false);

        BuildInventorySlots();
        amountText.text = FishInventoryService.Instance.CalculateTotalCoin().ToString();
    }

    public void OpenShopTab()
    {
        Debug.Log("상점탭 전환");
        coinTabRoot.SetActive(false);
        shopTabRoot.SetActive(true);
    }

    public void BuildInventorySlots()
    {
        Debug.Log($"FishType 개수: {System.Enum.GetValues(typeof(FishType)).Length}");
        // 1. 기존 슬롯 제거
        foreach (Transform child in coinGridRoot)
        {
            Destroy(child.gameObject);
        }
        // 2. 보관함 데이터 가져오기 (fishInventoryService 참고)
        var service = FishInventoryService.Instance;
        if (service == null) return;

        var fishList = service.GetFishViewList();
        if (fishList == null) return;

        // 3. 슬롯 생성하기 (도감 슬롯 기준)
        Dictionary<FishType, int> inventoryMap = new();
        foreach (var fish in fishList)
        {
            inventoryMap[fish.fishType] = fish.count;
        }

        int totalSlotCount = 12; // Todo: SO로 분리하여 원하는 만큼 슬롯을 늘릴 수 있음
        FishType[] fishTypes = (FishType[])System.Enum.GetValues(typeof(FishType));

        for (int i = 0; i < totalSlotCount; i++)
        {
            GameObject obj = Instantiate(fishToCoinSlotPrefab, coinGridRoot);
            FishToCoinSlot slot = obj.GetComponent<FishToCoinSlot>();

            if (i < fishTypes.Length)
            {
                FishType fishType = fishTypes[i];

                if (inventoryMap.TryGetValue(fishType, out int count) && count > 0)
                {
                    Sprite icon = service.GetFishSprite(fishType);
                    if (icon != null)
                    {
                        slot.Set(icon, count, fishType);
                    }
                    else
                    {
                        slot.SetEmpty(emptySprite);
                    }
                }
                else
                {
                    slot.SetEmpty(emptySprite);
                }
            }
            else
            {
                slot.SetEmpty(emptySprite);
            }
        }

    }

    public void OnClickExchange()
    {
        popupManager.ShowExchange(ExchangeFishToCoin);
    }

    public void ExchangeFishToCoin()
    {
        var service = FishInventoryService.Instance;
        if (service == null) return;

        int gained = service.CalculateTotalCoin();

        service.ExchangeAllFishToCoin();

        BuildInventorySlots();
        amountText.text = $"{gained:N0}개";

        UpdateCoinUI();


        Debug.Log($"[교환 완료] {gained} 코인 획득");
    }

    public void UpdateCoinUI()
    {
        int current = CoinService.Instance.CurrentCoin;
        harpoonCoinText.text = $"{current} / {RequiredCoin}";
    }

    public void BuyHarpoonUpgrade()
    {
        int current = CoinService.Instance.CurrentCoin;

        // 코인부족 시
        if (current < RequiredCoin)
        {
            Debug.Log("코인이 부족합니다");
            popupManager.ShowCoinFail();
            return;
        }

        // 이미 보유중인지 확인 여부
        if (SaveManager.Instance.HasHarpoonUpgrade())
        {
            Debug.Log("보유 중인 아이템입니다.");
            popupManager.ShowHasHarpoon();
            return;
        }

        // 구매 성공시
        CoinService.Instance.SpendCoin(RequiredCoin);
        SaveManager.Instance.SetHarpoonUpgrade(true);

        UpdateCoinUI();

        Debug.Log("구매가 완료되었습니다.");

    }
}