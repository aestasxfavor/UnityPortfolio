using System.Collections.Generic;
using UnityEngine;

public class FishInventoryService : MonoBehaviour
{
    public static FishInventoryService Instance;

    private FishInventoryData inventoryData;
    private InventoryUI inventoryUI;

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
        inventoryData = GameManager.Instance.GetSharedInventoryData();

        inventoryUI = FindObjectOfType<InventoryUI>();
    }

    public void SetInventoryUI(InventoryUI ui)
    {
        inventoryUI = ui;
    }
    public void AddFish(FishType fishType)
    {
        if (!fishSpriteCache.TryGetValue(fishType, out Sprite sprite))
        {
            sprite = LoadFishSprite(fishType);
            if (sprite == null) return;

            fishSpriteCache.Add(fishType, sprite);

        }
        inventoryData.AddFish(fishType, sprite);

        inventoryUI?.AddItemToUI(fishType, sprite);

        if(inventoryData ==null)
        {
            inventoryData = GameManager.Instance?.GetSharedInventoryData();
            if(inventoryData ==null)
            {
                Debug.LogError("inventoryData is Null");
                return;
            }
        }
    }

    private Sprite LoadFishSprite(FishType fishType)
    {
        var info = FishDataLoader.Instance.GetFishInfo(fishType);
        if (info == null) return null;

        return Resources.Load<Sprite>(info.worldSpritePath);
    }


}
