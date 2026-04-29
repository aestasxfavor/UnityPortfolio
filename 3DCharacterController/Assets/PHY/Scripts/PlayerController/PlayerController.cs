using UnityEngine;

/// <summary>
/// 2026-04-21
/// 리지드바디 기반 컨트롤러에서 캐릭터 컨트롤러 + 리지드바디 일부를 사용한 캐릭터 컨트롤러 제작 예정
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    private Transform cameraTransform;

    [Header("Controllers")]
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private MovementController movementController;
    [SerializeField] private JumpController jumpController;
    [SerializeField] private GroundDetector groundDetector;

    [Header("StatePatterns")]
    [SerializeField] private PlayerStateMachine playerStateMachine;

    [Header("Air State")]
    [SerializeField] private float fallThreshold = -0.5f;
    [SerializeField] private float fallStartDistance = 1.0f;

    [Header("Debug")]
    [SerializeField] private bool showDebugLog = true;

    [Header("Property")]
    public InputHandler InputHandler => inputHandler;
    public MovementController MovementController => movementController;
    public JumpController JumpController => jumpController;
    public GroundDetector GroundDetector => groundDetector;

    public float FallThreshold => fallThreshold;
    public float FallStartDistance => fallStartDistance;

    public float JumpStartY => jumpStartY;
    private float jumpStartY;

    public bool WasGrounded => wasGrounded;
    private bool wasGrounded;

    private void Reset()
    {
        characterController = GetComponent<CharacterController>();
        inputHandler = GetComponent<InputHandler>();
        movementController = GetComponent<MovementController>();
        jumpController = GetComponent<JumpController>();
        groundDetector = GetComponent<GroundDetector>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void Awake()
    {
        if (characterController == null) characterController = GetComponent<CharacterController>();
        if (inputHandler == null) inputHandler = GetComponent<InputHandler>();
        if (movementController == null) movementController = GetComponent<MovementController>();
        if (jumpController == null) jumpController = GetComponent<JumpController>();
        if (groundDetector == null) groundDetector = GetComponent<GroundDetector>();
        if (playerStateMachine == null) playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void Start()
    {
        groundDetector.GroundCheck();
        wasGrounded = groundDetector.IsGrounded;

        if (playerStateMachine != null)
        {
            playerStateMachine.ChangeState(new PlayerIdleState(this, playerStateMachine));
        }
    }

    public void SetCameraTransform(Transform newCameraTransform)
    {
        cameraTransform = newCameraTransform;
    }

    private void Update()
    {
        if (!CanRun()) return;

        wasGrounded = groundDetector.IsGrounded;

        groundDetector.GroundCheck();
        jumpController.UpdateVertical(characterController.isGrounded, Time.deltaTime);

        ProcessMovement();

        if (showDebugLog)
        {
            Debug.Log($"Grounded : {groundDetector.IsGrounded} | Walkable : {groundDetector.IsWalkableSlope} | SlopeAngle : {groundDetector.SlopeAngle}");
        }
    }

    private void ProcessMovement()
    {
        Vector3 moveDirection = movementController.GetMoveDirection(inputHandler.Movement, cameraTransform);

        bool hasMoveInput = moveDirection.sqrMagnitude > 0.01f;

        Vector3 horizontalMove = movementController.GetHorizontalMove(moveDirection, Time.deltaTime, groundDetector.IsGrounded);

        Vector3 finalMove = horizontalMove;
        finalMove.y = jumpController.VerticalVelocity;

        characterController.Move(finalMove * Time.deltaTime);

        if (!hasMoveInput) return;

        if (groundDetector.IsGrounded)
        {
            movementController.Rotate(transform, moveDirection, Time.deltaTime);
        }

        if (showDebugLog)
        {
            Debug.Log(
                $"GD_Grounded:{groundDetector.IsGrounded} | " +
                $"CC_Grounded:{characterController.isGrounded} | " +
                $"Walkable:{groundDetector.IsWalkableSlope} | " +
                $"SlopeAngle:{groundDetector.SlopeAngle:F2} | " +
                $"YSpeed:{jumpController.VerticalVelocity:F2}"
            );
        }
    }

    public void SetJumpStartY(float value)
    {
        jumpStartY = value;
    }

    private bool CanRun()
    {
        return characterController != null
            && inputHandler != null
            && movementController != null
            && jumpController != null
            && groundDetector != null
            && cameraTransform != null;
    }
}