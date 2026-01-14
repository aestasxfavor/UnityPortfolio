using UnityEngine;
using UnityEngine.SceneManagement;

public class FishTransformManager : MonoBehaviour
{
    [Header("바다 인벤토리 데이터 (InventoryUI에서 사용 중인 ScriptableObject)")]
    [SerializeField] private FishInventoryData seaInventoryData;   // 바다 인벤토리 (9칸+27칸 한 세트)

    [Header("육지 보관함 데이터")]
    [SerializeField] private FishInventoryData landStorageData;    // 육지 보관함 (누적용)

    public void ReturnToLandScene()
    {
        Debug.Log("[FishTransformManager] 산소 0 → 육지로 이동 시작");

        // 바다 인벤토리 → 육지 보관함으로 병합 이동
        seaInventoryData.TransferTo(landStorageData);

        // 씬 전환
        SceneManager.LoadScene("LandScene");
    }
}
