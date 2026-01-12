using System;
using System.Collections.Generic;
using UnityEngine;

public class FishInventoryService : MonoBehaviour
{
    public static FishInventoryService Instance;
    public event Action OnInventoryChanged;
    private FishInventoryData inventoryData;


    private Dictionary<FishType, Sprite> fishSpriteCache = new();
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
        Debug.Log($"[AddFish] µé¾î¿Â fishType = {fishType}");
        if (!fishSpriteCache.TryGetValue(fishType, out Sprite sprite))
        {
            sprite = LoadFishSprite(fishType);
            if (sprite == null) return;

            fishSpriteCache.Add(fishType, sprite);

        }


        if (inventoryData == null)
        {
            inventoryData = GameManager.Instance?.GetStorageInventoryData();
            if (inventoryData == null)
            {
                Debug.LogError("inventoryData is Null");
                return;
            }
        }
        inventoryData.AddFish(fishType, sprite);

        OnInventoryChanged?.Invoke();
    }

    private Sprite LoadFishSprite(FishType fishType)
    {
        var info = FishDataLoader.Instance.GetFishInfo(fishType);
        if (info == null) return null;

        return Resources.Load<Sprite>(info.worldSpritePath);
    }

    public void SetInventoryData(FishInventoryData data)
    {
        inventoryData = data;
        OnInventoryChanged?.Invoke();
    }
}
