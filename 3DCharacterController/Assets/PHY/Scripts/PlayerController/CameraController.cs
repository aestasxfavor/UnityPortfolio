using Unity.Cinemachine;
using UnityEngine;

/// <summary>
/// Todo: 마우스로 카메라 각도 조절 기능 추가 예정
/// </summary>

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform yawTarget;
    [SerializeField] private Transform pitchTarget;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private InputHandler inputHandler;

    [SerializeField] private float mouseSensitivity = 0.08f;
    [SerializeField] private float minPitch = -25f;
    [SerializeField] private float maxPitch = 60f;

    private float yaw;
    private float pitch;
    private bool useMouseLook = true;


    private void Reset()
    {
        inputHandler = GetComponent<InputHandler>();

        if (yawTarget == null)
        {
            Transform found = transform.Find("TrackingCamera_CloseBackView");
            if (found != null)
            {
                yawTarget = found;
            }
        }

        if (pitchTarget == null)
        {
            if (yawTarget != null)
            {
                Transform found = yawTarget.Find("CameraPitchTarget");

                if (found != null)
                {
                    pitchTarget = found;
                }
            }

            if (pitchTarget == null)
            {
                Transform found = transform.Find("CameraPitchTarget");

                if (found != null)
                {
                    pitchTarget = found;
                }
            }

        }
    }

    private void Awake()
    {
        if (inputHandler == null) inputHandler = GetComponent<InputHandler>();

    }

    private void Start()
    {
        BindCamera();
        InitRotation();
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null && Camera.main != null)
        {
            playerController.SetCameraTransform(Camera.main.transform);
        }
    }

    private void LateUpdate()
    {
        if (!useMouseLook)
        {
            return;
        }

        if (inputHandler == null || yawTarget == null)
        {
            return;
        }

        Vector2 lookInput = inputHandler.Look;

        yaw += lookInput.x * mouseSensitivity;
        pitch -= lookInput.y * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        yawTarget.rotation = Quaternion.Euler(0f, yaw, 0f);

        if (pitchTarget != null)
        {
            pitchTarget.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }

    public void BindCamera()
    {
        if (cinemachineCamera == null)
        {
            cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        }

        if (yawTarget == null)
        {
            Debug.LogWarning("CameraTarget을 찾지 못함");
            return;
        }

        if (cinemachineCamera == null)
        {
            Debug.LogWarning("CinemachineCamera를 찾지 못함");
            return;
        }

        if (cinemachineCamera != null && yawTarget != null)
        {
            cinemachineCamera.Target.TrackingTarget = yawTarget;
        }
        //cinemachineCamera.Target.TrackingTarget = pitchTarget != null ? pitchTarget : yawTarget;
    }

    private void InitRotation()
    {
        if (yawTarget != null)
        {
            yaw = yawTarget.eulerAngles.y;
        }

        if (pitchTarget != null)
        {
            pitch = Normalize(pitchTarget.eulerAngles.x);
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
    }

    private float Normalize(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        }

        return angle;
    }
}