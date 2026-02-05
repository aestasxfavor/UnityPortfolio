using System.Collections.Generic;
using UnityEngine;

public class CodexService : MonoBehaviour
{
    private CodexSaveData codexSaveData;

    private HashSet<FishType> discoveredFish;

    public void Init(CodexSaveData saveData)
    {
        codexSaveData = saveData;
        discoveredFish = new HashSet<FishType>();
    }

    public bool IsDiscovered(FishType type)
    {
        return discoveredFish != null && discoveredFish.Contains(type);
    }

    public void RegishterFish(FishType type)
    {

    }
}
