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

    private FishSpawner fishSpawner;
    private OxygenManager oxygenManager;
    private InventoryUI inventoryUI;

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
            SoundManager.Instance.PlaySFX(SoundManager.Instance.waterSplashSFX);
        }
    }


    public void Initialize(FishInventoryData seaData)
    {
        if(playerPrefab !=null)
        {
            playerInstance = Instantiate(playerPrefab);
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
    }

    public void CleanUp()
    {
        if (playerInstance != null)
        {
            Destroy(playerInstance);
        }

        if(oceanMapInstance != null)
        {
            Destroy(oceanMapInstance);
        }

        if(fishSpawner != null)
        {
            Destroy(fishSpawner.gameObject);
        }

        if(oxygenManager != null)
        {
            Destroy(oxygenManager.gameObject);
        }

        if(inventoryUI !=null)
        {
            Destroy(inventoryUI.gameObject);
        }

        playerInstance = null;
        oceanMapInstance = null;
        fishSpawner = null;
        oxygenManager = null;
        inventoryUI = null;
    }

    private IEnumerator LateStart()
    {
        yield return null;
        fishSpawner?.FishSpawn();
    }



}
