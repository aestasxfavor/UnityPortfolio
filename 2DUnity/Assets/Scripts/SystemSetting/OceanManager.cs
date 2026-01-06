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
        Debug.Log("[OceanManager] Initialize 완료");
    }

    private void Start() => StartCoroutine(LateStart());

    private IEnumerator LateStart()
    {
        yield return null;
        fishSpawner?.FishSpawn();
        oxygenManager?.ResetOxygen();
    }

    // 물고기 잡았을 때 호출

    // TODO: FishInventoryService로 이전 예정
    // 이 메서드는 리팩토링 후 제거 대상
    //public void AddCaughtFish(string fishName)
    //{
    //    if (inventoryUI == null)
    //    {
    //        Debug.LogError("[OceanManager] InventoryUI 연결 안 됨 ❌");
    //        return;
    //    }

    //    var info = FishDataLoader.Instance.GetFishInfo(fishName);
    //    if (info == null)
    //    {
    //        Debug.LogWarning($"[OceanManager] {fishName} 정보를 찾을 수 없습니다 ❌");
    //        return;
    //    }

    //    // 스프라이트 로드 (경로 예비 fallback 포함)
    //    Sprite fishSprite = Resources.Load<Sprite>(info.worldSpritePath);
    //    if (fishSprite == null)
    //        fishSprite = Resources.Load<Sprite>($"Sprites/Fish/{fishName}");

    //    if (fishSprite == null)
    //    {
    //        Debug.LogWarning($"[OceanManager] {fishName} 스프라이트 로드 실패");
    //        return;
    //    }

    //    if (!System.Enum.TryParse(fishName, out FishType fishType))
    //    {
    //        Debug.LogWarning($"[OceanManager] {fishName} → FishType 변환 실패");
    //        return;
    //    }

    //    // 1) 공용 데이터에 즉시 저장
    //    var gm = GameManager.Instance;
    //    if (gm != null)
    //    {
    //        var sharedData = gm.GetSharedInventoryData();
    //        if (sharedData != null)
    //        {
    //            sharedData.AddFish(fishType, fishSprite);
    //            Debug.Log($"[OceanManager] {fishType} → sharedInventoryData에 추가 완료 ✅");
    //        }
    //        else
    //            Debug.LogWarning("[OceanManager] sharedInventoryData가 null임");
    //    }

    //    // 2) 바다 인벤토리 UI 갱신
    //    inventoryUI.AddItemToUI(fishType, fishSprite);
    //}
}
