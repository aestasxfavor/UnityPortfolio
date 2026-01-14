using System.Collections;
using UnityEngine;

public class OceanManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FishSpawner fishSpawner;
    [SerializeField] private OxygenManager oxygenManager;
    [SerializeField] private InventoryUI inventoryUI;

    public void Initialize(FishSpawner spawner, OxygenManager oxygen, InventoryUI inv)
    {
        fishSpawner = spawner;
        oxygenManager = oxygen;
        inventoryUI = inv;
       // Debug.Log("[OceanManager] Initialize 완료");
    }

    private void Start() => StartCoroutine(LateStart());

    private IEnumerator LateStart()
    {
        yield return null;
        fishSpawner?.FishSpawn();
        oxygenManager?.ResetOxygen();
    }

}
