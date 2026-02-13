using System;
using System.Collections.Generic;
using UnityEngine;


public class FishInventoryService : MonoBehaviour
{
    public static FishInventoryService Instance;
    public event Action OnInventoryChanged;

    private FishInventoryData inventoryData;
    private CodexService codexService;

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
        var data = Data;
        if (data == null)
        {
            return;
        }

        data.AddFish(fishType);

        codexService?.RegisterFish(fishType);

        OnInventoryChanged?.Invoke();
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

        var info = loader.GetFishInfo(fishType);
        if (info == null) return null;

        if (string.IsNullOrEmpty(info.worldSpritePath))
            return null;

        var sprites = Resources.LoadAll<Sprite>(info.worldSpritePath);
        if (sprites == null || sprites.Length == 0)
        {
            // Debug.LogError($"[FishInventoryService] LoadAll<Sprite> fail path={info.worldSpritePath} (fishType={fishType})");
            return null;
        }

        // JSON에 iconSpriteName이 있으면 그걸로 고정 매핑
        if (!string.IsNullOrEmpty(info.iconSpriteName))
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                var s = sprites[i];
                if (s != null && s.name == info.iconSpriteName)
                    return s;
            }

            // Debug.LogError($"[FishInventoryService] target sprite not found: {info.iconSpriteName} / path={info.worldSpritePath}");
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

    // IReadOnlyList 이게 머여 
    public IReadOnlyList<FishViewData> GetFishViewList()
    {
        var data = Data;
        if (data == null) return null;

        var result = new List<FishViewData>();

        foreach (var slot in data.caughtFishList)
        {
            if (slot == null) continue;

            result.Add(new FishViewData(slot.fishType, slot.count));
        }

        return result;
    }
}
