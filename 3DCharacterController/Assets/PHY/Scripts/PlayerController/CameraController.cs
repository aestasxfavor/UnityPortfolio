using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    private void Reset()
    {
        if (cameraTarget == null)
        {
            Transform found = transform.Find("CameraTarget");
            if (found != null)
            {
                cameraTarget = found;
            }
        }
    }

    private void Awake()
    {
        if (cameraTarget == null)
        {
            Transform found = transform.Find("CameraTarget");
            if (found != null)
            {
                cameraTarget = found;
            }
        }
    }

    private void Start()
    {
        BindCamera();

        PlayerController playerController = GetComponent<PlayerController>();
        if(playerController != null && Camera.main != null)
        {
            playerController.SetCameraTransform(Camera.main.transform);
        }
    }

    public void BindCamera()
    {
        if (cinemachineCamera == null)
        {
            cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        }

        if (cameraTarget == null)
        {
            Debug.LogWarning("CameraTarget을 찾지 못함");
            return;
        }

        if (cinemachineCamera == null)
        {
            Debug.LogWarning("CinemachineCamera를 찾지 못함");
            return;
        }

        cinemachineCamera.Target.TrackingTarget = cameraTarget;
    }
}