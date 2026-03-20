using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region Prefab References (프리팹 참조)
    [Header("Prefabs")]
    [SerializeField] private GameObject fishDataLoaderPrefab;
    #endregion

    #region Cached Instances (캐시된 인스턴스)
    private GameObject fishDataLoaderInstance;
    #endregion

    #region Shared Inventory Data (공용 인벤토리 데이터)
    [Header("공용 인벤토리 데이터")]
    [SerializeField] private FishInventoryData storageInventoryData;
    [SerializeField] private FishInventoryData seaInventoryData;

    public FishInventoryData GetStorageInventoryData() => storageInventoryData;
    public FishInventoryData GetSeaInventoryData() => seaInventoryData;
    #endregion

    #region State (상태)
    public bool IsSceneLoading { get; private set; }
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

        CreateFishDataLoaderIfNeeded();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    #endregion

    #region Loader Setup (로더 세팅)
    private void CreateFishDataLoaderIfNeeded()
    {
        if (FishDataLoader.Instance != null)
            return;

        if (fishDataLoaderPrefab == null)
            return;

        if (fishDataLoaderInstance == null)
        {
            fishDataLoaderInstance = Instantiate(fishDataLoaderPrefab);
        }
    }
    #endregion

    #region Scene Setup (씬 세팅)
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        IsSceneLoading = false;
        StartCoroutine(SetUpSceneAfterLoad(scene));
    }

    private IEnumerator SetUpSceneAfterLoad(Scene scene)
    {
        yield return null;

        if (scene.name == "Ocean")
        {
            SetUpOceanScene();
        }
        else if (scene.name == "Land")
        {
            SetUpLandScene();
        }
    }

    private void SetUpOceanScene()
    {
        var currentSeaInventoryData = GetSeaInventoryData();
        if (currentSeaInventoryData == null)
            return;

        currentSeaInventoryData.Clear();

        FishInventoryService.Instance?.SetInventoryData(currentSeaInventoryData);

        var oceanManager = FindFirstObjectByType<OceanManager>();
        oceanManager?.SetUpOceanScene(currentSeaInventoryData);
    }

    private void SetUpLandScene()
    {
        if (seaInventoryData == null || storageInventoryData == null)
            return;

        seaInventoryData.TransferTo(storageInventoryData);

        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.Save();
        }

        if (FishInventoryService.Instance != null)
        {
            FishInventoryService.Instance.SetInventoryData(storageInventoryData);
        }

        var landUIManager = FindFirstObjectByType<LandUIManager>();
        landUIManager?.SetUpLand();
    }
    #endregion

    #region Scene Navigation (씬 이동)
    public void GoToOcean()
    {
        LoadScene("Ocean");
    }

    public void GoToLand()
    {
        LoadScene("Land");
    }

    public void GoToFadeScene(SceneType targetScene)
    {
        if (IsSceneLoading) return;

        IsSceneLoading = true;

        UnlockFirstReturnMailIfNeeded(targetScene);
        LoadScene(targetScene.ToString());
    }

    private void UnlockFirstReturnMailIfNeeded(SceneType targetScene)
    {
        if (targetScene != SceneType.Land)
            return;

        if (SaveManager.Instance == null)
            return;

        var mailboxSaveData = SaveManager.Instance.GetMailboxSaveData();
        if (mailboxSaveData.firstReturnMailUnlocked)
            return;

        mailboxSaveData.firstReturnMailUnlocked = true;
        SaveManager.Instance.RequestSave();
    }

    private void LoadScene(string sceneName)
    {
        if (SceneryManager.Instance != null)
        {
            SceneryManager.Instance.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
    #endregion
}