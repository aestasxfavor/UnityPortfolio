using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [SerializeField] private FishInventoryData storageInventoryData;
    [SerializeField] private MailboxSaveData mailboxSaveData = new();

    private CodexSaveData codexSaveData = new CodexSaveData();

    private string savePath;
    private string mailPath;
    private string codexPath;

    private float saveTimer;
    private float saveCoolDown = 10f;

    private bool saveRequest;



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
        mailPath = Path.Combine(Application.persistentDataPath, "mail_save.json");
        codexPath = Path.Combine(Application.persistentDataPath, "codex_save.json");
        Debug.Log($"세이브 경로 : " + savePath);

        if (mailboxSaveData == null)
            mailboxSaveData = new MailboxSaveData();

    }


    void Start()
    {
        if (GameManager.Instance != null)
        {
            storageInventoryData = GameManager.Instance.GetStorageInventoryData();
        }

        if (storageInventoryData == null) return;

        Load();
        LoadMail();
        LoadCodex();
    }

    private void Update()
    {
        if (!saveRequest) return;

        saveTimer += Time.unscaledDeltaTime;
        if (saveTimer < saveCoolDown) return;

        saveTimer = 0f;
        saveRequest = false;

        Save();
        SaveMail();
        SaveCodex();
    }

    public void RequestSave()
    {
        saveRequest = true;
    }

    public void Save()
    {
        if (storageInventoryData == null)
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
        if (!File.Exists(savePath))
        {
            return;
        }

        string json = File.ReadAllText(savePath);

        SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

        if (loadedData == null)
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
    #region 메일 Json
    public MailboxSaveData GetMailboxSaveData()
    {
        return mailboxSaveData;
    }
    public void SaveMail()
    {
        if (mailboxSaveData == null)
            mailboxSaveData = new MailboxSaveData();

        string json = JsonUtility.ToJson(mailboxSaveData, true);
        File.WriteAllText(mailPath, json);
    }

    public void LoadMail()
    {
        if (!File.Exists(mailPath))
        {
            mailboxSaveData = new MailboxSaveData();
            return;
        }

        string json = File.ReadAllText(mailPath);
        mailboxSaveData = JsonUtility.FromJson<MailboxSaveData>(json);

        if (mailboxSaveData == null)
            mailboxSaveData = new MailboxSaveData();
    }
    #endregion

    #region 도감 Json
    public CodexSaveData GetCodexSaveData()
    {
        return codexSaveData;
    }

    public void SaveCodex()
    {
        string json = JsonUtility.ToJson(codexSaveData, true);
        File.WriteAllText(codexPath, json);
    }

    public void LoadCodex()
    {
        if (!File.Exists(codexPath))
        {
            codexSaveData = new CodexSaveData();
            return;
        } 

        string json = File.ReadAllText(codexPath);
        codexSaveData = JsonUtility.FromJson<CodexSaveData>(json);

        if(codexSaveData == null)
        {
            codexSaveData= new CodexSaveData();
        }

    }
    #endregion


}
