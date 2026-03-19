using Unity.Cinemachine;
using UnityEngine;

public class CameraBound : MonoBehaviour
{
    [SerializeField] private CinemachineCamera oceanCamera;

    public void SetTarget(Transform target)
    {
        //player = target;
        if (oceanCamera == null || target == null) return;
        oceanCamera.Follow = target;
        Debug.Log($"[CameraBound] 카메라 대상 연결 완료 → {target.name}");

    }

    public void ClearTarget()
    {
        if(oceanCamera == null) return;

        oceanCamera.Follow = null;
    }
}
