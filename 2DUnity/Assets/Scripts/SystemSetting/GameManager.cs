using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject oceanMapPrefab;
    public GameObject fishSpawnerPrefab;
    public GameObject oxygenUIPrefab;
    public GameObject inventoryUIPrefab;
    public GameObject fishDataLoaderPrefab;
    public GameObject storageUICanvasPrefab;
    public GameObject buttonCanvasPrefab;

    // Ocean 생성물 캐시
    private GameObject playerInstance;
    private GameObject oceanMapInstance;
    private FishSpawner spawnerInstance;
    private OxygenManager oxygenMgrInstance;
    private InventoryUI inventoryUIInstance;
    private GameObject fishDataLoaderInstance;

    // Land 생성물 캐시
    private GameObject storageUICanvasInstance;
    private StorageUI storageUIInstance;
    private GameObject buttonCanvasInstance;

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

        if (scene.name == "Ocean")
            yield return StartCoroutine(SetupOceanScene());
        else if (scene.name == "Land")
            SetupLandScene();
    }

    // -------------------------
    // Ocean Scene 세팅
    // -------------------------
    private IEnumerator SetupOceanScene()
    {
        // Todo: OceanManager가 수명 관리하도록 이동 예정
        CleanupOceanObjects();

        // Debug.Log("[GameManager] Ocean 씬 세팅 시작");

        // 바다 데이터 비우고 서비스가 바다 데이터 물게 하기
        var seaData = GetSeaInventoryData();
        if (seaData == null)
        {
            // Debug.LogError("[GameManager] seaInventoryData가 null입니다 (Inspector 연결 확인)");
            yield break;
        }
        seaData.Clear();

        // 겜매니저에 유지하기
        if (FishInventoryService.Instance != null)
            FishInventoryService.Instance.SetInventoryData(seaData);
        else
            Debug.LogWarning("[GameManager] FishInventoryService.Instance == null");

        // 플레이어 생성
        // Todo: OceanManager가 생성 책임을 가지도록 이동 가능 (선택)
        if (playerPrefab != null)
            playerInstance = Instantiate(playerPrefab);

        // Todo: OceanManager가 생성 책임을 가지도록 이동 가능 (선택)
        // 맵 생성
        if (oceanMapPrefab != null)
            oceanMapInstance = Instantiate(oceanMapPrefab);

        // Todo: OceanManager로 이동 예정
        // 물고기 스포너
        if (fishSpawnerPrefab != null)
            spawnerInstance = Instantiate(fishSpawnerPrefab).GetComponent<FishSpawner>();

        // Todo: OceanManager로 이동 예정
        // 산소 UI
        if (oxygenUIPrefab != null)
            oxygenMgrInstance = Instantiate(oxygenUIPrefab).GetComponent<OxygenManager>();

        // Todo: OceanManager로 이동 예정
        // 인벤토리 UI
        if (inventoryUIPrefab != null)
            inventoryUIInstance = Instantiate(inventoryUIPrefab).GetComponentInChildren<InventoryUI>(true);

        inventoryUIInstance?.SetInventoryData(seaData);

        //// 데이터 로더
        //if (fishDataLoaderPrefab != null)
        //    fishDataLoaderInstance = Instantiate(fishDataLoaderPrefab);

        yield return null; // 한 프레임 더 대기 (카메라와 오브젝트 로딩 기다림)

        // 카메라 연결
        CameraBound cam = FindFirstObjectByType<CameraBound>();
        if (cam != null && playerInstance != null)
        {
            cam.SetTarget(playerInstance.transform);
            //Debug.Log("[GameManager] CameraBound 플레이어 연결 완료");
        }
        else
        {
            //Debug.LogWarning("[GameManager] CameraBound 또는 Player를 찾을 수 없습니다");
        }

        // OceanManager 연결
        // Todo: 겜매니저에 유지하기
        var oceanManager = FindFirstObjectByType<OceanManager>();
        if (oceanManager != null)
        {
            oceanManager.Initialize(spawnerInstance, oxygenMgrInstance, inventoryUIInstance);
            // Debug.Log("[GameManager] OceanManager 초기화 완료");
        }
        else
        {
            // Debug.LogWarning("[GameManager] OceanManager를 찾을 수 없습니다");
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.waterSplashSFX);
        }
        else
        {
            // Debug.LogWarning("[GameManager] SoundManager.Instance == null");
        }

        Debug.Log("[GameManager] Ocean 씬 세팅 완료");
    }

    // Todo: OceanSceneManager가 씬 종료 시 정리하도록 이동 예정
    private void CleanupOceanObjects()
    {
        if (playerInstance != null) Destroy(playerInstance);
        if (oceanMapInstance != null) Destroy(oceanMapInstance);

        if (spawnerInstance != null) Destroy(spawnerInstance.gameObject);
        if (oxygenMgrInstance != null) Destroy(oxygenMgrInstance.gameObject);

        if (inventoryUIInstance != null) Destroy(inventoryUIInstance.transform.root.gameObject);
        //if (fishDataLoaderInstance != null) Destroy(fishDataLoaderInstance);

        playerInstance = null;
        oceanMapInstance = null;
        spawnerInstance = null;
        oxygenMgrInstance = null;
        inventoryUIInstance = null;
        //fishDataLoaderInstance = null;
    }

    // -------------------------
    // Land Scene 세팅
    // -------------------------
    private void SetupLandScene()
    {
        // Debug.Log("[GameManager] Land 씬 세팅 시작");

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

        // Storage UI (중복 생성 방지)
        // Todo: LandUIManager로 이동 예정
        if (storageUICanvasInstance == null && storageUICanvasPrefab != null)
        {
            storageUICanvasInstance = Instantiate(storageUICanvasPrefab);
            storageUIInstance = storageUICanvasInstance.GetComponentInChildren<StorageUI>(true);
        }

        if (storageUIInstance != null)
        {
            storageUIInstance.SetInventoryData(storageInventoryData);
            //Debug.Log("[GameManager] StorageUI 데이터 연결 완료");
        }
        else
        {
            // Debug.LogWarning("[GameManager] StorageUI를 찾을 수 없습니다");
        }

        // Todo: LandUIManager로 이동 예정
        // 버튼 캔버스 (중복 생성 방지)
        if (buttonCanvasInstance == null && buttonCanvasPrefab != null)
        {
            buttonCanvasInstance = Instantiate(buttonCanvasPrefab);
        }

        if (buttonCanvasInstance != null)
        {
            buttonCanvasInstance.SetActive(true);

            var storageButton = buttonCanvasInstance.GetComponentInChildren<StorageButton>(true);
            if (storageButton != null && storageUIInstance != null)
            {
                storageButton.SetTargetStorage(storageUIInstance);
                // Debug.Log("[GameManager] StorageButton ↔ StorageUI 연결 완료");
            }
        }

        Debug.Log("[GameManager] Land 씬 세팅 완료");
    }

    public GameObject GetButtonCanvasInstance()
    {
        return buttonCanvasInstance;
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
