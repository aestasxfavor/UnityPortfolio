using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CodexUI : MonoBehaviour
{
    [SerializeField] private Transform codexPannel;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Sprite unknownIcon;

    private List<UISlot> slots = new();

    private void OnEnable()
    {
        BuildCodex();
    }

    private void BuildCodex()
    {
        foreach (var slot in slots)
        {
            Destroy(slot.gameObject);
        }
        slots.Clear();

        foreach (FishType type in System.Enum.GetValues(typeof(FishType)))
        {
            GameObject obj = Instantiate(slotPrefab, codexPannel);
            UISlot slot = obj.GetComponent<UISlot>();
            slots.Add(slot);

            bool discovered = SaveManager.Instance.IsFishDiscovered(type);

            if (discovered)
            {
                Sprite icon = FishInventoryService.Instance.GetFishSprite(type);
                slot.CodexDiscover(icon);
            }
            else
            {
                slot.CodexUndiscover(unknownIcon);
            }
        }
    }
}
