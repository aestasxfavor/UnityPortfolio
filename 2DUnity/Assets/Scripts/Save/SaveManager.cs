using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    #region Save Data References (세이브 데이터 참조)
    [SerializeField] private FishInventoryData storageInventoryData;
    [SerializeField] private MailboxSaveData mailboxSaveData = new();

    private CodexSaveData codexSaveData = new();
    private CoinSaveData coinSaveData = new();
    #endregion

    #region Save Paths (세이브 경로)
    private string inventorySavePath;
    private string mailboxSavePath;
    private string codexSavePath;
    private string coinSavePath;
    #endregion

    #region Upgrade State (업그레이드 상태)
    private bool hasHarpoonUpgrade;
    #endregion

    #region Singleton & Lifecycle (싱글톤 및 생명주기)
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeSavePaths();
        EnsureSaveDataObjects();
    }

    private void Start()
    {
        ConnectStorageInventoryData();
        LoadAll();
    }
    #endregion

    #region Initialization (초기화)
    private void InitializeSavePaths()
    {
        inventorySavePath = Path.Combine(Application.persistentDataPath, "fish_save.json");
        mailboxSavePath = Path.Combine(Application.persistentDataPath, "mail_save.json");
        codexSavePath = Path.Combine(Application.persistentDataPath, "codex_save.json");
        coinSavePath = Path.Combine(Application.persistentDataPath, "coin_save.json");
    }

    private void EnsureSaveDataObjects()
    {
        if (mailboxSaveData == null)
        {
            mailboxSaveData = new MailboxSaveData();
        }

        if (codexSaveData == null)
        {
            codexSaveData = new CodexSaveData();
        }

        if (coinSaveData == null)
        {
            coinSaveData = new CoinSaveData();
        }
    }

    private void ConnectStorageInventoryData()
    {
        if (GameManager.Instance != null)
        {
            storageInventoryData = GameManager.Instance.GetStorageInventoryData();
        }
    }
    #endregion

    #region Save All / Load All (전체 저장 / 전체 불러오기)
    public void RequestSave()
    {
        SaveAll();
    }

    public void SaveAll()
    {
        Save();
        SaveMailbox();
        SaveCodex();
        SaveCoin();
    }

    public void LoadAll()
    {
        Load();
        LoadMailbox();
        LoadCodex();
        LoadCoin();
    }
    #endregion

    #region Inventory Save (인벤토리 저장)
    public void Save()
    {
        if (storageInventoryData == null)
            return;

        SaveData saveData = new SaveData
        {
            hasHarpoonUpgrade = hasHarpoonUpgrade
        };

        foreach (var fish in storageInventoryData.caughtFishList)
        {
            if (fish == null)
                continue;

            saveData.caughtFishList.Add(new FishSaveSlot
            {
                fishType = fish.fishType.ToString(),
                count = fish.count
            });
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(inventorySavePath, json);
    }

    public void Load()
    {
        if (storageInventoryData == null)
            return;

        if (!File.Exists(inventorySavePath))
            return;

        string json = File.ReadAllText(inventorySavePath);
        SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

        if (loadedData == null)
            return;

        hasHarpoonUpgrade = loadedData.hasHarpoonUpgrade;

        storageInventoryData.caughtFishList.Clear();

        foreach (var slot in loadedData.caughtFishList)
        {
            storageInventoryData.AddFishSave(slot.fishType, slot.count);
        }
    }
    #endregion

    #region Mailbox Save (우편함 저장)
    public MailboxSaveData GetMailboxSaveData()
    {
        return mailboxSaveData;
    }

    public void SaveMailbox()
    {
        if (mailboxSaveData == null)
            mailboxSaveData = new MailboxSaveData();

        string json = JsonUtility.ToJson(mailboxSaveData, true);
        File.WriteAllText(mailboxSavePath, json);
    }

    public void LoadMailbox()
    {
        if (!File.Exists(mailboxSavePath))
        {
            mailboxSaveData = new MailboxSaveData();
            return;
        }

        string json = File.ReadAllText(mailboxSavePath);
        mailboxSaveData = JsonUtility.FromJson<MailboxSaveData>(json);

        if (mailboxSaveData == null)
        {
            mailboxSaveData = new MailboxSaveData();
        }
    }
    #endregion

    #region Codex Save (도감 저장)
    public CodexSaveData GetCodexSaveData()
    {
        return codexSaveData;
    }

    public void SaveCodex()
    {
        string json = JsonUtility.ToJson(codexSaveData, true);
        File.WriteAllText(codexSavePath, json);
    }

    public void LoadCodex()
    {
        if (!File.Exists(codexSavePath))
        {
            codexSaveData = new CodexSaveData();
            return;
        }

        string json = File.ReadAllText(codexSavePath);
        codexSaveData = JsonUtility.FromJson<CodexSaveData>(json);

        if (codexSaveData == null)
        {
            codexSaveData = new CodexSaveData();
        }
    }
    #endregion

    #region Coin Save (코인 저장)
    public CoinSaveData GetCoinSaveData()
    {
        return coinSaveData;
    }

    public void SaveCoin()
    {
        string json = JsonUtility.ToJson(coinSaveData, true);
        File.WriteAllText(coinSavePath, json);
    }

    public void LoadCoin()
    {
        if (!File.Exists(coinSavePath))
        {
            coinSaveData = new CoinSaveData { currentCoin = 0 };
            return;
        }

        string json = File.ReadAllText(coinSavePath);
        coinSaveData = JsonUtility.FromJson<CoinSaveData>(json);

        if (coinSaveData == null)
        {
            coinSaveData = new CoinSaveData { currentCoin = 0 };
        }
    }
    #endregion

    #region Harpoon Upgrade State (작살 업그레이드 상태)
    public bool HasHarpoonUpgrade()
    {
        return hasHarpoonUpgrade;
    }

    public void SetHarpoonUpgrade(bool hasUpgrade)
    {
        hasHarpoonUpgrade = hasUpgrade;
        Save();
    }
    #endregion
}