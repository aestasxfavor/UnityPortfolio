using UnityEngine;

/// <summary>
/// Todo: 7단계 리팩토링에서 스크립트 삭제 예정
/// 현재 Button-> LandUIManager 연결용 스크립트
/// </summary>

public class StorageButton : MonoBehaviour
{
    //private StorageUI storageUI;
    private LandUIManager landUIManager;

    public void SetLandUIManager(LandUIManager manager)
    {
        landUIManager = manager;
    }

    //public void SetTargetStorage(StorageUI ui)
    //{
    //    storageUI = ui;
    //}

    public void OnClickOpenStorage()
    {
        //if (storageUI != null)
        //{
        //    storageUI.ToggleInventoryUI();
        //}
        //else
        //{
        //    Debug.LogWarning("[StorageButton] StorageUI 참조 없음");
        //}
        if (landUIManager != null)
        {
            landUIManager.OpenStorage();

        }
    }
}
