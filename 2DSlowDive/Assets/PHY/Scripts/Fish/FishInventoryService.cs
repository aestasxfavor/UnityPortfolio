using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class FishInventoryService : MonoBehaviour
{
    public static FishInventoryService Instance;
    public event Action OnInventoryChanged;

    private FishInventoryData inventoryData;
    private CodexService codexService;

    public int CurrentDiveCaughtCount { get; private set; }
    public int CurrentDiveNewDiscoveryCount {  get; private set; }

    private readonly Dictionary<FishType, Sprite> fishSpriteCache = new(128);

    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        inventoryData = GameManager.Instance.GetStorageInventoryData();
    }

    private FishInventoryData Data
    {
        get
        {
            if (inventoryData == null)
            {
                inventoryData = GameManager.Instance?.GetStorageInventoryData();
            }
            return inventoryData;
        }
    }

    public void Init(CodexService service)
    {
        codexService = service;
    }


    public void AddFish(FishType fishType)
    {
        var storageData = Data;
        if (storageData == null)
        {
            return;
        }

        storageData.AddFish(fishType);

        // 이번 잠수에서 회수한 전체 물고기 수
        CurrentDiveCaughtCount++;

        // 신규어종이면 true, 이미 발견한 어종이면 false
        bool isNewDiscovery = codexService?.Registerfish(fishType) ?? false;

        if (isNewDiscovery) CurrentDiveNewDiscoveryCount++;

        //codexService?.RegisterFish(fishType);

        OnInventoryChanged?.Invoke();
    }

    public void ResetDiveResult()
    {
        CurrentDiveCaughtCount = 0;
        CurrentDiveNewDiscoveryCount = 0;
    }

    public Sprite GetFishSprite(FishType fishType)
    {
        if (fishSpriteCache.TryGetValue(fishType, out var sprite))
        {
            return sprite;

        }
        sprite = LoadFishSprite(fishType);

        if (sprite != null)
            fishSpriteCache[fishType] = sprite;

        return sprite;
    }

    private Sprite LoadFishSprite(FishType fishType)
    {
        var loader = FishDataLoader.Instance;
        if (loader == null) return null;

        var fishInfo = loader.GetFishInfo(fishType);
        if (fishInfo == null) return null;

        if (string.IsNullOrEmpty(fishInfo.worldSpritePath))
            return null;

        var sprites = Resources.LoadAll<Sprite>(fishInfo.worldSpritePath);
        if (sprites == null || sprites.Length == 0)
        { 
            return null;
        }

        // JSON에 iconSpriteName이 있으면 그걸로 고정 매핑
        if (!string.IsNullOrEmpty(fishInfo.iconSpriteName))
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                var sprite = sprites[i];
                if (sprite != null && sprite.name == fishInfo.iconSpriteName)
                    return sprite;
            }

            // Debug.LogError($"[FishInventoryService] target sprite not found: {fishInfo.iconSpriteName} / path={fishInfo.worldSpritePath}");
            return null;
        }

        // iconSpriteName 없으면 fallback (디버그 겸용)
        //Debug.LogWarning($"[FishInventoryService] iconSpriteName 비어있음 → fallback sprites[0] 사용 (fishType={fishType})");
        return sprites[0];
    }

    public void SetInventoryData(FishInventoryData data)
    {
        inventoryData = data;
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// IReadOnlyList: 읽기 전용 리스트 인터페이스 
    /// 물고기 목록을 보여주기 위한 데이터를 넘기기 위해 사용
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<FishViewData> GetFishViewList()
    {
        var inventoryData = Data;
        if (inventoryData == null) return null;

        var result = new List<FishViewData>();

        foreach (var slot in inventoryData.caughtFishList)
        {
            if (slot == null) continue;

            result.Add(new FishViewData(slot.fishType, slot.count));
        }

        return result;
    }

    #region 물고기 코인 교환 로직
    public int CalculateTotalCoin()  // 물고기 계산함수
    {
        var inventoryData = Data;
        if (inventoryData == null) return 0;
        int total = 0;
        foreach (var fish in inventoryData.caughtFishList)
        {
            if (fish == null) continue;
            total += fish.count;
        }

        return total * 10;
    }

    public int ExchangeAllFishToCoin()     // 교환실행 함수
    {
     
        // 1. 잡은 물고기 총 계산하기
        int coin = CalculateTotalCoin();

        // 2. 잡은 물고기 전부 비우기
        inventoryData.caughtFishList.Clear();

        // 3. 코인 증가시키기
        CoinService.Instance.AddCoin(coin);
        // 4. 저장하기 
        SaveManager.Instance.RequestSave();

        return coin;
    }
    #endregion
}