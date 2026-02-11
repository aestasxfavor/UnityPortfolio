using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Prefabs")]
    public GameObject fishDataLoaderPrefab;

    private GameObject fishDataLoaderInstance;

    [Header("공용 인벤토리 데이터 (씬 공통 사용)")]
    [SerializeField] private FishInventoryData storageInventoryData;
    [SerializeField] private FishInventoryData seaInventoryData;

    public FishInventoryData GetStorageInventoryData() => storageInventoryData;
    public FishInventoryData GetSeaInventoryData() => seaInventoryData;

    public bool IsSceneLoading { get; private set; }


    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        EnsureFishDataLoader();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void EnsureFishDataLoader()
    {
        if (FishDataLoader.Instance != null) return;

        if (fishDataLoaderPrefab == null)
        {
            return;
        }

        if (fishDataLoaderInstance == null)
        {
            fishDataLoaderInstance = Instantiate(fishDataLoaderPrefab);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        IsSceneLoading = false;
        StartCoroutine(SetupSceneDelayed(scene));
    }

    private IEnumerator SetupSceneDelayed(Scene scene)
    {
        yield return null; // 한 프레임 대기

        if(scene.name == "Ocean")
        {
            var seaData = GetSeaInventoryData();
            seaData.Clear();

            FishInventoryService.Instance?.SetInventoryData(seaData);

            var ocean = FindFirstObjectByType<OceanManager>();
            ocean?.SetUpOcean(seaData);
        }
        else if(scene.name =="Land")
        {
            SetupLandScene();
        }
    }

    // -------------------------
    // Land Scene 세팅 나중에 landUIManager로 옮기던지 LandManager로 옮기던지 둘중 하나할 예정
    // -------------------------
    private void SetupLandScene()
    {
        if (seaInventoryData == null || storageInventoryData == null)
        {
            // Debug.LogError("[GameManager] sea/storage InventoryData가 null입니다 (Inspector 연결 확인)");
            return;
        }

        // 바다 인벤 → 보관함 누적
        seaInventoryData.TransferTo(storageInventoryData);

        if (SaveManager.Instance != null)
            SaveManager.Instance.Save();

        // 서비스가 storage 물게 하기
        if (FishInventoryService.Instance != null)
            FishInventoryService.Instance.SetInventoryData(storageInventoryData);

        var landUI = FindFirstObjectByType<LandUIManager>();
        if (landUI != null)
            landUI.SetUpLand(storageInventoryData);
    }

    // -------------------------
    // 씬 이동
    // -------------------------
    public void GoToOcean()
    {
        if (SceneryManager.Instance != null)
            SceneryManager.Instance.LoadScene("Ocean");
        else
            SceneManager.LoadScene("Ocean");
    }

    public void GoToLand()
    {

        if (SceneryManager.Instance != null)
            SceneryManager.Instance.LoadScene("Land");
        else
            SceneManager.LoadScene("Land");
    }

    public void GoToFadeScene(SceneType targetScene)
    {
        Debug.Log($"[GameManager] 페이드 씬 전환 요청 → {targetScene}");

        if (IsSceneLoading) return;
        IsSceneLoading = true;

        // Todo: MailBoxService로 이동 예정
        if (targetScene == SceneType.Land && SaveManager.Instance != null)
        {
            var mail = SaveManager.Instance.GetMailboxSaveData();

            Debug.Log($"[MAIL] unlocked(before)={mail.firstReturnMailUnlocked}");

            if (!mail.firstReturnMailUnlocked)
            {
                mail.firstReturnMailUnlocked = true;
                SaveManager.Instance.RequestSave();
                Debug.Log($"[MAIL] unlocked(after)={mail.firstReturnMailUnlocked}");
            }
        }

        string sceneName = targetScene.ToString();

        var sceneryManager = FindFirstObjectByType<SceneryManager>();
        if (sceneryManager != null)
            sceneryManager.LoadScene(sceneName);
        else
            SceneManager.LoadScene(sceneName);
    }
}
