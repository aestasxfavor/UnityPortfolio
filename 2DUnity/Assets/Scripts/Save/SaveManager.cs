using System.IO;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static SaveManager Instance { get; private set; }
   [SerializeField] private FishInventoryData storageInventoryData;

    private string savePath;

    private float saveTimer;
    private bool saveRequest;

    private float saveCoolDown = 10f;
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


    void Start()
    {
        if(GameManager.Instance != null)
        {
            storageInventoryData = GameManager.Instance.GetStorageInventoryData();
        }

        if (storageInventoryData == null) return;
            
        Load();
    }

    private void Update()
    {
        if (!saveRequest) return;

        saveTimer += Time.unscaledDeltaTime;
        if (saveTimer < saveCoolDown) return;

        saveTimer = 0f;
        saveRequest = false;

        Save();
    }

    public void RequestSave()
    {
        saveRequest = true;
    }

    public void Save()
    {
        if(storageInventoryData == null)
        {
            return;
        }

        SaveData saveData = new SaveData();

        foreach (var fish in storageInventoryData.caughtFishList)
        {
            if (fish == null) continue;
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

            storageInventoryData.caughtFishList.
            Clear();

        foreach (var slot in loadedData.caughtFishList)
        {
            storageInventoryData.AddFishSave(slot.fishType, slot.count);
        }
    }
}
