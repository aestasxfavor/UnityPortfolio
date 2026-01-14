using System;
using System.Collections.Generic;
using UnityEngine;

public class FishInventoryService : MonoBehaviour
{
    public static FishInventoryService Instance;
    public event Action OnInventoryChanged;

    private FishInventoryData inventoryData;

    private readonly Dictionary<FishType, Sprite> fishSpriteCache = new();

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

    public void AddFish(FishType fishType)
    {
        if (inventoryData == null)
        {
            inventoryData = GameManager.Instance?.GetStorageInventoryData();
            if (inventoryData == null)
            {
               // Debug.LogError("[FishInventoryService] inventoryData is null");
                return;
            }
        }

        inventoryData.AddFish(fishType);
        OnInventoryChanged?.Invoke();
    }

    public Sprite GetFishSprite(FishType fishType)
    {
        if (!fishSpriteCache.TryGetValue(fishType, out var sprite))
        {
            sprite = LoadFishSprite(fishType);
            if (sprite != null)
                fishSpriteCache[fishType] = sprite;
        }
        return sprite;
    }

    private Sprite LoadFishSprite(FishType fishType)
    {
        if (FishDataLoader.Instance == null)
        {
           // Debug.LogError($"[FishInventoryService] FishDataLoader.Instance == null (fishType={fishType})");
            return null;
        }

        var info = FishDataLoader.Instance.GetFishInfo(fishType);
        if (info == null)
        {
          //  Debug.LogError($"[FishInventoryService] FishInfo null (fishType={fishType})");
            return null;
        }

        if (string.IsNullOrEmpty(info.worldSpritePath))
        {
           // Debug.LogError($"[FishInventoryService] worldSpritePath empty (fishType={fishType})");
            return null;
        }

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
                if (sprites[i] != null && sprites[i].name == info.iconSpriteName)
                    return sprites[i];
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
}
