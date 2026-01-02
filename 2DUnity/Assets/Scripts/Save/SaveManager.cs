using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static SaveManager Instance { get; private set; }
   [SerializeField] private FishInventoryData fishInventoryData;

    private string savePath;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "fish_save.json");
        Debug.Log($"세이브 경로 : " + savePath);

    }

    // Update is called once per frame
    void Start()
    {
        Load();
    }

   public void Save()
    {
        if(fishInventoryData == null)
        {
            return;
        }

        SaveData saveData = new SaveData();

        foreach (var fish in fishInventoryData.caughtFishList)
        {
            saveData.caughtFishList.Add(new FishSaveSlot { fishType = fish.fishType.ToString(), count = fish.count });
        }

        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(savePath, json);
        Debug.Log("게임 저장 완료");
    }

    public void Load()
    {
        if(!File.Exists(savePath))
        {
            return;
        }

        string json = File.ReadAllText(savePath);

        SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

        if(loadedData == null)
        {
            return;
        }

        fishInventoryData.caughtFishList.Clear();

        foreach (var slot in loadedData.caughtFishList)
        {
            fishInventoryData.AddFishSave(slot.fishType, slot.count);
        }
    }
}
