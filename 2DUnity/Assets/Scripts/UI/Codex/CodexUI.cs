using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// 현재 UI가 SaveManager에 직접 접근 중
// 추후 CodexService 분리 시 도감 관련 로직을 Service로 이관 예정 (완료)


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

            bool discovered = codexService.IsDiscovered(type);
            //Debug.Log($"[CodexUI] slots count = {slots.Count}, childCount = {codexPannel.childCount}");


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

    //public void CodexToggle()
    //{
    //    if (codexService == null) return;
    //    gameObject.SetActive(!gameObject.activeSelf);
    //}

}
