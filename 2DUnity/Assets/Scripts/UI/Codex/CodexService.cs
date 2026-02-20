using System.Collections.Generic;
using UnityEngine;


public class CodexService : MonoBehaviour
{
    private CodexSaveData codexSaveData;

    private HashSet<FishType> discoveredFish;

    public bool IsInitialized { get; private set; }

    public void Init(CodexSaveData saveData)
    {
        Debug.Log("[CodexService] Init CALLED");
        codexSaveData = saveData ?? new CodexSaveData();

        if(codexSaveData.codexFishID == null)
        {
            codexSaveData.codexFishID = new List<string>();
        }
        discoveredFish = new HashSet<FishType>();

        foreach (var fishID in codexSaveData.codexFishID)
        {
            if (System.Enum.TryParse(fishID, out FishType type))
            {
                discoveredFish.Add(type);
            }
        }

        IsInitialized = true;
    }

    public bool IsDiscovered(FishType type)
    {
        if (!IsInitialized)
        {
            return false;
        }

        return discoveredFish != null && discoveredFish.Contains(type);
    }

    public void RegisterFish(FishType type)
    {
        //Debug.Log($"[CodexService] RegisterFish CALLED : {type}, initialized={IsInitialized}");

        // 1. 이미 등록됐는지 체크
        if(discoveredFish.Contains(type))
        {
            Debug.Log($"[CodexService] 이미 등록된 어종:{type}");
            return;
        }
        if (!IsInitialized) return;
        // 2. 새로 발견이면 상태 추가
        if (!discoveredFish.Add(type)) return;

        // 3. CodexSaveData에 반영
        codexSaveData.codexFishID.Add(type.ToString());

        // 4. SaveManager에게 “저장 요청”만 보내기
        SaveManager.Instance.RequestSave();
    }

    public List<CodexViewData> GetCodexViewList()
    {
        var list = new List<CodexViewData>();
        if(!IsInitialized) return list;

        int index = 1;

        foreach (FishType type in System.Enum.GetValues(typeof(FishType)))
        {
            bool discovered = IsDiscovered(type);

            Sprite icon = null;
            if (discovered)
                icon = FishInventoryService.Instance.GetFishSprite(type);

            list.Add(new CodexViewData(type, icon, discovered, index));
            index++;
        }

        return list;
    }

}
