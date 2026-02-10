using UnityEngine;

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
