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
    public GameObject warningUIPrefab;
    public GameObject fishDataLoaderPrefab;
    public GameObject storageUICanvasPrefab;
    public GameObject buttonCanvasPrefab;

    private GameObject playerInstance;
    private GameObject oceanMapInstance;
    private GameObject buttonCanvasInstance;

    [Header("공용 인벤토리 데이터 (씬 공통 사용)")]
    [SerializeField] private FishInventoryData fishInventoryData;
    public FishInventoryData GetSharedInventoryData() => fishInventoryData;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        Debug.Log($"[GameManager] 초기화 완료 — fishInventoryData 해시: {fishInventoryData?.GetHashCode()}");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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

    // Ocean Scene 세팅
    // TODO: SceneFlow / SceneInstaller 분리 대상
    private IEnumerator SetupOceanScene()
    {
        Debug.Log("[GameManager] Ocean 씬 세팅 시작");

        // 플레이어 생성
        if (playerPrefab != null)
            playerInstance = Instantiate(playerPrefab);

        // 맵 생성
        if (oceanMapPrefab != null)
            oceanMapInstance = Instantiate(oceanMapPrefab);

        // 물고기 스포너
        FishSpawner spawner = null;
        if (fishSpawnerPrefab != null)
            spawner = Instantiate(fishSpawnerPrefab).GetComponent<FishSpawner>();

        // 산소 UI
        OxygenManager oxygenMgr = null;
        if (oxygenUIPrefab != null)
            oxygenMgr = Instantiate(oxygenUIPrefab).GetComponent<OxygenManager>();

        // 인벤토리 UI
        InventoryUI inventoryUI = null;
        if (inventoryUIPrefab != null)
            inventoryUI = Instantiate(inventoryUIPrefab).GetComponentInChildren<InventoryUI>(true);

        // 경고 UI
        if (warningUIPrefab != null)
            Instantiate(warningUIPrefab);

        // 데이터 로더
        if (fishDataLoaderPrefab != null)
            Instantiate(fishDataLoaderPrefab);

        yield return null; // 한 프레임 더 대기 (카메라와 오브젝트 로딩 기다림)

        // 카메라 연결
        CameraBound cam = FindFirstObjectByType<CameraBound>();
        if (cam != null && playerInstance != null)
        {
            cam.SetTarget(playerInstance.transform);
            Debug.Log("[GameManager] CameraBound 플레이어 연결 완료");
        }
        else
        {
            Debug.LogWarning("[GameManager] CameraBound 또는 Player를 찾을 수 없습니다");
        }

        // OceanManager 연결
        var oceanManager = FindObjectOfType<OceanManager>();
        if (oceanManager != null)
        {
            oceanManager.Initialize(spawner, oxygenMgr, inventoryUI);
            Debug.Log("[GameManager] OceanManager 초기화 완료");
        }
        else
        {
            Debug.LogWarning("[GameManager] OceanManager를 찾을 수 없습니다");
        }

        SoundManager.Instance.PlaySFX(SoundManager.Instance.waterSplashSFX);

        Debug.Log("[GameManager] Ocean 씬 세팅 완료");
    }

    // Land Scene 세팅
    private void SetupLandScene()
    {
        Debug.Log("[GameManager] Land 씬 세팅 시작");

        GameObject storageUIObj = Instantiate(storageUICanvasPrefab);
        var storageUI = storageUIObj.GetComponentInChildren<StorageUI>(true);
        if (storageUI != null)
        {
            storageUI.SetInventoryData(fishInventoryData);
            Debug.Log("[GameManager] StorageUI 생성 및 데이터 연결 완료");
        }

        if (buttonCanvasPrefab != null)
        {
            buttonCanvasInstance = Instantiate(buttonCanvasPrefab); // 저장
            buttonCanvasInstance.SetActive(true); // 기본 활성화

            var storageButton = buttonCanvasInstance.GetComponentInChildren<StorageButton>(true);
            if (storageButton != null && storageUI != null)
            {
                storageButton.SetTargetStorage(storageUI);
                Debug.Log("[GameManager] StorageButton ↔ StorageUI 연결 완료");
            }
        }

        Debug.Log("[GameManager] Land 씬 세팅 완료");
    }

    public GameObject GetButtonCanvasInstance()
    {
        return buttonCanvasInstance;
    }

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

    public void GoToFadeScene()
    {
        Debug.Log("페이드씬 전환 요청");

        var sceneryManager = FindObjectOfType<SceneryManager>();
        if (sceneryManager != null)
            sceneryManager.LoadScene("Land");
        else
            SceneManager.LoadScene("Land");
    }
}
