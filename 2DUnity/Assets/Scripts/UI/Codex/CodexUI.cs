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

        var list = codexService.GetCodexViewList();

        foreach (var data in list)
        {
            GameObject obj = Instantiate(slotPrefab, codexPannel);
            CodexUISlot slot = obj.GetComponent<CodexUISlot>();

            slots.Add(slot);

            if(data.discovered)
            {
                slot.SetDiscovered(data.icon, data.index);
            }
            else
            {
                slot.SetUndiscovered(unknownIcon, data.index);
            }
        }
    }

}
