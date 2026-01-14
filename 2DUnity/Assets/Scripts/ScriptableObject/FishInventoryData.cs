using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishInventoryData", menuName = "Game/Fish Inventory Data")]
public class FishInventoryData : ScriptableObject // 일반 c# 클래스로 분리 예정
{
    [System.Serializable]
    public class FishSlot
    {
        public FishType fishType;
        //public Sprite fishIcon;
        public int count;
    }

    public List<FishSlot> caughtFishList = new List<FishSlot>();

    /// <summary>
    /// 새로운 물고기 추가 (공용 인벤토리 반영)
    /// </summary>
    public void AddFish(FishType type)
    {
        var existing = caughtFishList.Find(f => f.fishType == type);
        if (existing != null)
        {
            existing.count++;

            return;
        }
        else
        {
            caughtFishList.Add(new FishSlot { fishType = type, count = 1 });
        }

       // Debug.Log($"FishInventoryData: {type} ");
        SaveManager.Instance.Save();
    }


    /// <summary>
    /// 전체 초기화 (씬 이동 시 데이터 싹 비움)
    /// </summary>
    public void Clear()
    {
        caughtFishList.Clear();
       // Debug.Log("[FishInventoryData] 전체 데이터 초기화 완료");
    }

    /// <summary>
    /// 바다 → 육지 전송용 (공유 인벤토리 ↔ 보관함)
    /// </summary>
    public void TransferTo(FishInventoryData targetInventory)
    {
        if (targetInventory == null)
        {
           // Debug.LogWarning("[FishInventoryData] TransferTo 실패 - targetInventory 없음");
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
                targetInventory.caughtFishList.Add(new FishSlot 
                { fishType = fish.fishType, count = fish.count });
            }

            movedCount++;
        }

       // Debug.Log($"[FishInventoryData] {targetInventory.name}으로 {movedCount}종 이동 완료");
        Clear(); // 이동 후 초기화
    }

    public void AddFishSave(string typeString, int count)
    {
        if (!System.Enum.TryParse(typeString, out FishType type))
        {
            return;
        }

        var exisiting = caughtFishList.Find(f => f.fishType == type);

        if (exisiting != null)
        {
            exisiting.count = count;
            return;
        }

        caughtFishList.Add(new FishSlot {fishType = type, count = count });

    }
}
