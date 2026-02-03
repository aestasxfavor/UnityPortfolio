using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CodexUI : MonoBehaviour
{
    [SerializeField] private Transform codexPannel;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Sprite unknownIcon;


    private List<CodexUISlot> slots = new();

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

        int index = 1;

        foreach (FishType type in System.Enum.GetValues(typeof(FishType)))
        {
            GameObject obj = Instantiate(slotPrefab, codexPannel);
            CodexUISlot slot = obj.GetComponent<CodexUISlot>();
            
            if(slot ==null)
            {
                continue;
            }

            slots.Add(slot);

            bool discovered = SaveManager.Instance.IsFishDiscovered(type);

            if (discovered)
            {
                Sprite icon = FishInventoryService.Instance.GetFishSprite(type);
                slot.SetDiscovered(icon, index);

            }
            else
            {
                slot.SetUndiscovered(unknownIcon, index);
            }
            index++;
        }
    }

    public void CodexToggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

}
