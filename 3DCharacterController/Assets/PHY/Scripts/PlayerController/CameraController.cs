using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform yawTarget;
    [SerializeField] private Transform pitchTarget;
    [SerializeField] private Transform cameraSocket;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private InputHandler inputHandler;

    [SerializeField] private float mouseSensitivity = 0.08f;
    [SerializeField] private float minPitch = -25f;
    [SerializeField] private float maxPitch = 60f;
    [SerializeField] private bool useMouseLook = true;

    private float yaw;
    private float pitch;

    private void Reset()
    {
        FindReferences();
    }

    private void Awake()
    {
        FindReferences();
    }

    private void Start()
    {
        BindCameraTarget();
        InitCameraRotation();

        PlayerController playerController = GetComponent<PlayerController>();

        if (playerController != null && Camera.main != null)
        {
            playerController.SetCameraTransform(Camera.main.transform);
        }
    }

    private void LateUpdate()
    {
        if (!useMouseLook) return;
        if (inputHandler == null || yawTarget == null) return;

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

    private void FindReferences()
    {
        if (inputHandler == null)
        {
            inputHandler = GetComponent<InputHandler>();
        }

        if (cinemachineCamera == null)
        {
            cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        }

        if (yawTarget == null)
        {
            yawTarget = transform.Find("TrackingCamera_CloseBackView");
        }

        if (pitchTarget == null && yawTarget != null)
        {
            pitchTarget = yawTarget.Find("CameraPitchTarget");
        }

        if (pitchTarget == null)
        {
            pitchTarget = transform.Find("CameraPitchTarget");
        }

        if (cameraSocket == null && pitchTarget != null)
        {
            cameraSocket = pitchTarget.Find("CameraSocket");
        }
    }

    private void BindCameraTarget()
    {
        FindReferences();

        if (yawTarget == null)
        {
            Debug.LogWarning("yawTarget을 찾지 못함");
            return;
        }

        if (pitchTarget == null)
        {
            Debug.LogWarning("pitchTarget을 찾지 못함");
            return;
        }

        if (cameraSocket == null)
        {
            Debug.LogWarning("cameraSocket을 찾지 못함");
            return;
        }

        if (cinemachineCamera == null)
        {
            Debug.LogWarning("시네머신 카메라를 찾지 못함");
            return;
        }

        cinemachineCamera.Target.TrackingTarget = cameraSocket;
    }

    private void InitCameraRotation()
    {
        if (yawTarget != null)
        {
            yaw = yawTarget.eulerAngles.y;
        }

        if (pitchTarget != null)
        {
            pitch = NormalizeAngle(pitchTarget.eulerAngles.x);
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f)
        {
            angle -= 360f;
        }

        return angle;
    }
}