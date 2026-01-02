using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishInventoryData", menuName = "Game/Fish Inventory Data")]
public class FishInventoryData : ScriptableObject
{
    [System.Serializable]
    public class FishSlot
    {
        public FishType fishType;
        public Sprite fishIcon;
        public int count;
    }

    public List<FishSlot> caughtFishList = new List<FishSlot>();

    /// <summary>
    /// 새로운 물고기 추가 (공용 인벤토리 반영)
    /// </summary>
    public void AddFish(FishType type, Sprite icon)
    {
        var existing = caughtFishList.Find(f => f.fishType == type);
        if (existing != null)
        {
            existing.count++;

            SaveManager.Instance.Save();
            return;
        }

        // 반드시 Resources에서 다시 로드 시도
        Sprite sprite = icon;
        if (sprite == null)
        {
            string path = $"Fish/{type}";
            Sprite[] loadedSprites = Resources.LoadAll<Sprite>(path);
            if (loadedSprites.Length > 0)
            {
                sprite = loadedSprites[0];
                Debug.Log($"[FishInventoryData] {path} 첫 스프라이트 로드 성공");
            }
            else
            {
                sprite = Resources.Load<Sprite>(path);
                Debug.LogWarning($"[FishInventoryData] {path} 로드 실패");
            }
        }

        caughtFishList.Add(new FishSlot
        {
            fishType = type,
            fishIcon = sprite,   // 아이콘 null 방지
            count = 1
        });

        Debug.Log($"[FishInventoryData] {type} 추가 완료 아이콘={(sprite != null)}");

        SaveManager.Instance.Save();
    }


    /// <summary>
    /// 전체 초기화 (씬 이동 시 데이터 싹 비움)
    /// </summary>
    public void Clear()
    {
        caughtFishList.Clear();
        Debug.Log("[FishInventoryData] 전체 데이터 초기화 완료");
    }

    /// <summary>
    /// 바다 → 육지 전송용 (공유 인벤토리 ↔ 보관함)
    /// </summary>
    public void TransferTo(FishInventoryData targetInventory)
    {
        if (targetInventory == null)
        {
            Debug.LogWarning("[FishInventoryData] TransferTo 실패 - targetInventory 없음");
            return;
        }

        int movedCount = 0;

        foreach (var fish in caughtFishList)
        {
            if (fish == null) continue;

            var existing = targetInventory.caughtFishList.Find(f => f.fishType == fish.fishType);
            if (existing != null)
            {
                existing.count += fish.count;
            }
            else
            {
                // 아이콘 누락 시 재로드
                Sprite sprite = fish.fishIcon;
                if (sprite == null)
                {
                    string path = $"Sprites/Fish/{fish.fishType}";
                    sprite = Resources.Load<Sprite>(path);
                    Debug.Log($"[FishInventoryData] {fish.fishType} 전송 중 아이콘 재로드됨");
                }

                targetInventory.caughtFishList.Add(new FishSlot
                {
                    fishType = fish.fishType,
                    fishIcon = sprite,
                    count = fish.count
                });
            }

            movedCount++;
        }

        Debug.Log($"[FishInventoryData] {targetInventory.name}으로 {movedCount}종 이동 완료");
        Clear(); // 이동 후 초기화
    }

    public void AddFishSave(string typeString, int count)
    {
        if (!System.Enum.TryParse(typeString, out FishType type))
        {
            return;
        }

        string path = $"Fish/{type}";
        Sprite sprite = null;

        Sprite[] loaded = Resources.LoadAll<Sprite>(path);
        if (loaded.Length > 0)
        {
            sprite = loaded[0];
        }
        else
        {
            sprite = Resources.Load<Sprite>(path);
        }

        var exisiting = caughtFishList.Find(f => f.fishType == type);

        if (exisiting != null)
        {
            exisiting.count = count;

            if (exisiting.fishIcon == null)
            {
                exisiting.fishIcon = sprite;
            }

            return;
        }

        caughtFishList.Add(new FishSlot { fishIcon = sprite, fishType = type, count = count });

    }
}
