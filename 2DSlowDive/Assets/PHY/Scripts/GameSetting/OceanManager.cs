using System.Collections;
using UnityEngine;

public class OceanManager : MonoBehaviour
{
    #region Prefab References (프리팹)
    [Header("Ocean Prefabs")]
    [SerializeField] private GameObject fishSpawnerPrefab;
    [SerializeField] private GameObject oxygenUIPrefab;
    [SerializeField] private GameObject inventoryUIPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject oceanMapPrefab;
    [SerializeField] private GameObject harpoonUIPrefab;
    #endregion

    #region Scene References (카메라 참조)
    [Header("Camera Ref")]
    [SerializeField] private CameraBound cameraBound;
    #endregion

    #region Cached Components (캐시 생성물 컴포넌트)
    private FishSpawner fishSpawner;
    private OxygenManager oxygenManager;
    private InventoryUI inventoryUI;
    private HarpoonUI harpoonUI;
    #endregion

    #region Cached Instances (캐시 생성물)
    private GameObject playerInstance;
    private GameObject oceanMapInstance;
    private GameObject inventoryUIInstance;
    private GameObject oxygenUIInstance;
    private GameObject harpoonUIInstance;
    #endregion

    #region Ocean Setup (바다 씬 초기 세팅)
    public void SetUpOceanScene(FishInventoryData seaData)
    {
        DestroyOceanObjects();
        CreateOceanObjects(seaData);
        BindCamera();
        StartCoroutine(SpawnFishNextFrame());

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StopSwimSFX();
            SoundManager.Instance.PlayWaterSplashSFX();
        }
    }

    private void CreateOceanObjects(FishInventoryData seaData)
    {
        CreatePlayerIfNeeded();
        CreateOceanMapIfNeeded();
        CreateFishSpawnerIfNeeded();
        CreateOxygenUIIfNeeded();
        CreateInventoryUIIfNeeded(seaData);
        CreateHarpoonUIIfNeeded();
    }

    private void CreatePlayerIfNeeded()
    {
        if (playerPrefab != null)
        {
            playerInstance = Instantiate(playerPrefab);
        }
    }

    private void CreateOceanMapIfNeeded()
    {
        if (oceanMapPrefab != null)
        {
            oceanMapInstance = Instantiate(oceanMapPrefab);
        }
    }

    private void CreateFishSpawnerIfNeeded()
    {
        if (fishSpawnerPrefab != null)
        {
            fishSpawner = Instantiate(fishSpawnerPrefab).GetComponent<FishSpawner>();
        }
    }

    private void CreateOxygenUIIfNeeded()
    {
        if (oxygenUIPrefab != null)
        {
            oxygenUIInstance = Instantiate(oxygenUIPrefab);
            oxygenManager = oxygenUIInstance.GetComponentInChildren<OxygenManager>(true);
        }
    }

    private void CreateInventoryUIIfNeeded(FishInventoryData seaData)
    {
        if (inventoryUIPrefab != null)
        {
            inventoryUIInstance = Instantiate(inventoryUIPrefab);
            inventoryUI = inventoryUIInstance.GetComponentInChildren<InventoryUI>(true);
            inventoryUI?.SetInventoryData(seaData);
        }
    }

    private void CreateHarpoonUIIfNeeded()
    {
        if (harpoonUIPrefab != null)
        {
            harpoonUIInstance = Instantiate(harpoonUIPrefab);
            harpoonUI = harpoonUIInstance.GetComponentInChildren<HarpoonUI>(true);
        }
    }

    private void BindCamera()
    {
        if (cameraBound == null || playerInstance == null)
            return;

        cameraBound.SetTarget(playerInstance.transform);
    }

    private IEnumerator SpawnFishNextFrame()
    {
        yield return null;
        fishSpawner?.FishSpawn();
    }
    #endregion

    #region Ocean UI Transition Control (바다 UI 전환 제어)
    public void HideOceanUIForTransition()
    {
        if (oxygenUIInstance != null)
        {
            oxygenUIInstance.SetActive(false);
        }

        if (inventoryUIInstance != null)
        {
            inventoryUIInstance.SetActive(false);
        }

        if (harpoonUIInstance != null)
        {
            harpoonUIInstance.SetActive(false);
        }
    }
    #endregion

    #region Ocean Reset & Cleanup (바다씬 리셋 & 정리)
    public void ResetOcean()
    {
        oxygenManager?.ResetOxygen();
        inventoryUI?.Close();
        fishSpawner?.ResetSpawn();
    }

    public void DestroyOceanObjects()
    {
        if (cameraBound != null)
        {
            cameraBound.ClearTarget();
        }

        if (playerInstance != null)
        {
            Destroy(playerInstance);
        }

        if (oceanMapInstance != null)
        {
            Destroy(oceanMapInstance);
        }

        if (fishSpawner != null)
        {
            Destroy(fishSpawner.gameObject);
        }

        if (oxygenUIInstance != null)
        {
            Destroy(oxygenUIInstance);
        }

        if (inventoryUIInstance != null)
        {
            Destroy(inventoryUIInstance);
        }

        if (harpoonUIInstance != null)
        {
            Destroy(harpoonUIInstance);
        }

        playerInstance = null;
        oceanMapInstance = null;
        inventoryUIInstance = null;
        oxygenUIInstance = null;
        harpoonUIInstance = null;

        fishSpawner = null;
        oxygenManager = null;
        inventoryUI = null;
        harpoonUI = null;
    }
    #endregion
}