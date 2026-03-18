using System.Collections;
using UnityEngine;

public class OceanManager : MonoBehaviour
{
    [Header("Ocean Prefabs")]
    [SerializeField] private GameObject fishSpawnerPrefab;
    [SerializeField] private GameObject oxygenUIPrefab;
    [SerializeField] private GameObject inventoryUIPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject oceanMapPrefab;
    [SerializeField] private GameObject harpoonUIPrefab;

    private FishSpawner fishSpawner;
    private OxygenManager oxygenManager;
    private InventoryUI inventoryUI;
    private HarpoonUI harpoonUI;

    private GameObject playerInstance;
    private GameObject oceanMapInstance;

    public void ResetOcean() 
    {
        Debug.Log("ResetOcean 호출됨");
        oxygenManager?.ResetOxygen();
        inventoryUI?.Close();
        fishSpawner?.ResetSpawn();
    }

    public void SetUpOcean(FishInventoryData seaData)
    {
        CleanUp();
        Initialize(seaData);
        StartCoroutine(LateStart());

        CameraBound cam = FindFirstObjectByType<CameraBound>();
        if (cam != null && playerInstance != null)
        {
            cam.SetTarget(playerInstance.transform);
        }

        if (SoundManager.Instance != null)
        {
            // SoundManager.Instance.PlaySwimSFX();
            SoundManager.Instance.StopSwimSFX();
            SoundManager.Instance.PlayWaterSplashSFX();
        }
    }


    public void Initialize(FishInventoryData seaData)
    {
        PlayerAim playerAim = null;

        if (playerPrefab != null)
        {
            playerInstance = Instantiate(playerPrefab);
            playerAim = playerInstance.GetComponent<PlayerAim>();
        }

        if (oceanMapPrefab != null)
        {
            oceanMapInstance = Instantiate(oceanMapPrefab);
        }

        if (fishSpawnerPrefab != null)
        {
            fishSpawner = Instantiate(fishSpawnerPrefab).GetComponent<FishSpawner>();
        }

        if (oxygenUIPrefab != null)
        {
            oxygenManager = Instantiate(oxygenUIPrefab).GetComponent<OxygenManager>();
        }

        if (inventoryUIPrefab != null)
        {
            inventoryUI = Instantiate(inventoryUIPrefab).GetComponentInChildren<InventoryUI>(true);
            inventoryUI?.SetInventoryData(seaData);
        }

        if (harpoonUIPrefab != null)
        {
            harpoonUI = Instantiate(harpoonUIPrefab).GetComponent<HarpoonUI>();
           
        }
    }

    public void CleanUp()
    {
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

        if (oxygenManager != null)
        {
            Destroy(oxygenManager.gameObject);
        }

        if (inventoryUI != null)
        {
            Destroy(inventoryUI.gameObject);
        }

        if (harpoonUI != null)
        {
            Destroy(harpoonUI.gameObject);
        }

        playerInstance = null;
        oceanMapInstance = null;
        fishSpawner = null;
        oxygenManager = null;
        inventoryUI = null;
        harpoonUI = null;
    }

    private IEnumerator LateStart()
    {
        yield return null;
        fishSpawner?.FishSpawn();
    }



}
