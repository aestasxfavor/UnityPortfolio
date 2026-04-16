using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    private Transform cameraTransform;

    [Header("Controllers")]
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private MovementController movementController;
    [SerializeField] private JumpController jumpController;
    [SerializeField] private GroundDetector groundDetector;

    [Header("StatePatterns")]
    [SerializeField] private PlayerStateMachine playerStateMachine;

    public InputHandler InputHandler => inputHandler;
    public MovementController MovementController => movementController;
    public JumpController JumpController => jumpController;
    public GroundDetector GroundDetector => groundDetector;

    [SerializeField] private bool showDebugLog = true;

    // 인스펙터 자동 참조
    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        movementController = GetComponent<MovementController>();
        jumpController = GetComponent<JumpController>();
        groundDetector = GetComponent<GroundDetector>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    // 필수 컴포넌트 참조 보정
    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (inputHandler == null) inputHandler = GetComponent<InputHandler>();
        if (movementController == null) movementController = GetComponent<MovementController>();
        if (jumpController == null) jumpController = GetComponent<JumpController>();
        if (groundDetector == null) groundDetector = GetComponent<GroundDetector>();
        if (playerStateMachine == null) playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void Start()
    {
        if (playerStateMachine != null)
        {
            playerStateMachine.ChangeState(new PlayerIdleState(this, playerStateMachine));
        }
    }

    public void SetCameraTransform(Transform newCameraTransform)
    {
        cameraTransform = newCameraTransform;
    }

    private void FixedUpdate()
    {
        if (!CanRun()) return;

        groundDetector.GroundCheck();

        ProcessMovement();
        ProcessJump();

        jumpController.UpdateVertical(groundDetector.IsGrounded, Time.fixedDeltaTime);

        Vector3 velocity = rb.linearVelocity;
        velocity.y = jumpController.VerticalVelocity;
        rb.linearVelocity = velocity;

        if (showDebugLog)
        {
            Debug.Log($"Grounded : {groundDetector.IsGrounded} | Walkable : {groundDetector.IsWalkableSlope} | SlopeAngle : {groundDetector.SlopeAngle}");
        }
    }

    // 이동 입력 처리 로직
    private void ProcessMovement()
    {
        Vector3 moveDirection = movementController.GetMoveDirection(inputHandler.Movement, cameraTransform);

        bool isBlockedBySlope = groundDetector.IsGrounded && !groundDetector.IsWalkableSlope;

        if (isBlockedBySlope)
        {
            movementController.Move(rb, Vector3.zero);
            return;
        }

        movementController.Move(rb, moveDirection);
        movementController.Rotate(transform, moveDirection, Time.fixedDeltaTime);
    }

    // 점프 입력 처리 로직
    private void ProcessJump()
    {
        if (!inputHandler.JumpPressed) return;

        bool canJump = groundDetector.IsGrounded && groundDetector.IsWalkableSlope;

        if (canJump)
        {
            jumpController.TryJump(canJump);
            playerStateMachine.ChangeState(new PlayerJumpState(this, playerStateMachine));
        }

        inputHandler.ResetJumpInput();
    }

    // 필수 참조 확인
    private bool CanRun()
    {
        return rb != null
            && inputHandler != null
            && movementController != null
            && jumpController != null
            && groundDetector != null
            && cameraTransform != null;


    }
}