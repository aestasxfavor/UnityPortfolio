using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishInventoryData", menuName = "Game/Fish Inventory Data")]
public class FishInventoryData : ScriptableObject // 일반 c# 클래스로 분리 예정 => 분리해도되나? 
{
    [System.Serializable]
    public class FishSlot
    {
        public FishType fishType;
        public int count;
    }

    public List<FishSlot> caughtFishList = new List<FishSlot>();

    /// <summary>
    /// 새로운 물고기 추가 (공용 인벤토리 반영)
    /// </summary>
    public void AddFish(FishType type)
    {
        for (int i = 0; i < caughtFishList.Count; i++)
        {
            var slot = caughtFishList[i];
            if(slot != null && slot.fishType == type)
            {
                slot.count++;
                return;
            }
        }

        caughtFishList.Add(new FishSlot  { fishType = type, count = 1 });

        
        SaveManager.Instance.RequestSave();
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
    public void TransferTo(FishInventoryData otherInventory)
    {
        if (otherInventory == null)
        {
            return;
        }

        foreach (var fish in caughtFishList)
        {
            if (fish == null) continue;

            FishSlot sameSlot = null;

            for (int i = 0; i < otherInventory.caughtFishList.Count; i++)
            {
                var slot = otherInventory.caughtFishList[i];
                if(slot != null && slot.fishType == fish.fishType)
                {
                    sameSlot = slot;
                    break;
                }
            }

            if (sameSlot != null)
            {
                sameSlot.count += fish.count;
            }
            else
            {
                otherInventory.caughtFishList.Add(new FishSlot { fishType = fish.fishType, count = fish.count });
            }
        }

        Clear(); // 이동 후 초기화
    }

    public void AddFishSave(string typeString, int count)
    {
        if (!System.Enum.TryParse(typeString, out FishType type))
        {
            return;
        }

        for (int i = 0; i < caughtFishList.Count; i++)
        {
            var slot = caughtFishList[i];
            if(slot != null && slot.fishType == type)
            {
                slot.count = count;
                return;
            }
        }

        caughtFishList.Add(new FishSlot {fishType = type, count = count });

    }
}
