using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StorageConfigSO", menuName = "Scriptable Objects/StorageConfigSO")]
public class StorageConfigSO : ScriptableObject
{
    public List<FishType> slotFishOrder = new();
    public int totalSlots = 27;

    private void FishFill()
    {
        slotFishOrder = Enum.GetValues(typeof(FishType)).Cast<FishType>().ToList();

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
        Debug.Log($"[StorageConfigSO] slotFishOrder 자동 채우기 완료 ({slotFishOrder.Count}개)");
    }
}

