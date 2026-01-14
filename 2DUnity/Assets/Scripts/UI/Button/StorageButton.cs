using UnityEngine;

public class StorageButton : MonoBehaviour
{
    private StorageUI storageUI;

    public void SetTargetStorage(StorageUI ui)
    {
        storageUI = ui;
    }

    public void OnClickOpenStorage()
    {
        if (storageUI != null)
        {
            storageUI.ToggleInventoryUI();
           // Debug.Log("[StorageButton] 보관함 열기 성공");
        }
        else
        {
            Debug.LogWarning("[StorageButton] StorageUI 참조 없음");
        }
    }
}
