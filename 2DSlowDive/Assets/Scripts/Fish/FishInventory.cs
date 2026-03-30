using System.Collections.Generic;
using UnityEngine;

public class FishInventory : MonoBehaviour
{
    public static FishInventory Instance;

    private Dictionary<string, int> caughtFishDict = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddFish(string fishName)
    {
        if (caughtFishDict.ContainsKey(fishName))
        {
            caughtFishDict[fishName]++;
        }
        else
        {
            caughtFishDict[fishName] = 1;
        }

       // Debug.Log($"{fishName} 잡음! 현재 수량: {caughtFishDict[fishName]}");
    }

    public int GetFishCount(string fishName)
    {
        if (caughtFishDict.TryGetValue(fishName, out int count))
            return count;
        return 0;
    }

    public void PrintInventory()
    {
        //Debug.Log("[현재 인벤토리]");
        foreach (var fish in caughtFishDict)
        {
            //Debug.Log($"- {fish.Key}: {fish.Value}마리");
        }
    }
}
