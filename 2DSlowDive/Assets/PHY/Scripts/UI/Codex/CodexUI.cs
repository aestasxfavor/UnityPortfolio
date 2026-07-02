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
    private CodexService codexService;

    public void Open()
    {
        Debug.Log("도감 열림");

        if (codexService != null)
        {
            codexService.MarkAllBang();
        }
        TryBuild();

    }

    public void Init(CodexService service)
    {
        codexService = service;
        TryBuild();
    }

    private void TryBuild()
    {
        if (codexService == null) return;
        if (!codexService.IsInitialized) return;

        BuildCodex();
    }

    private void BuildCodex()
    {
        foreach (var slot in slots)
        {
            Destroy(slot.gameObject);
        }
        slots.Clear();

        var codexViewList = codexService.GetCodexViewList();
        int totalSlotCount = 24;     // Todo: 리팩토링할 때 SO로 분리가능

        for (int i = 0; i < totalSlotCount; i++)
        {
            GameObject obj = Instantiate(slotPrefab, codexPannel);
            CodexUISlot slot = obj.GetComponent<CodexUISlot>();
            slots.Add(slot);

            if (i < codexViewList.Count)
            {
                var codexViewData = codexViewList[i];

                if (codexViewData.discovered)
                {
                    slot.SetDiscovered(codexViewData.icon, codexViewData.index);
                }
                else
                {
                    slot.SetUndiscovered(unknownIcon, codexViewData.index);
                }
            }
            else
            {
                slot.SetUndiscovered(unknownIcon, i + 1);
            }

        }
    }

}
