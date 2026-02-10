using System.Collections;
using UnityEngine;

public class OceanManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject fishSpawnerPrefab;
    [SerializeField] private GameObject oxygenUIPrefab;
    [SerializeField] private GameObject inventoryUIPrefab;

    private FishSpawner fishSpawner;
    private OxygenManager oxygenManager;
    private InventoryUI inventoryUI;

    public void ResetOcean()
    {
        //Todo: 내일 리팩토링 6단계 시작예정
        Debug.Log("ResetOcean 호출됨");
    }


    public void Initialize(FishInventoryData seaData)
    {
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
        }

        inventoryUI?.SetInventoryData(seaData);
    }

    private void Start() => StartCoroutine(LateStart());

    private IEnumerator LateStart()
    {
        yield return null;
        fishSpawner?.FishSpawn();
        oxygenManager?.ResetOxygen();
    }

}
