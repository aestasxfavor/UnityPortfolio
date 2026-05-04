using UnityEngine;

/// <summary>
/// 2026-04-21
/// 리지드바디 기반 컨트롤러에서 캐릭터 컨트롤러 + 리지드바디 일부를 사용한 캐릭터 컨트롤러 제작 예정
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerAnimationController playerAnimationController;
    private Transform cameraTransform;

    [Header("Controllers")]
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private MovementController movementController;
    [SerializeField] private JumpController jumpController;
    [SerializeField] private GroundDetector groundDetector;
    [SerializeField] private HighFallDetector highFallDetector;

    [Header("StatePatterns")]
    [SerializeField] private PlayerStateMachine playerStateMachine;

    [Header("Air State")]
    [SerializeField] private float fallThreshold = -0.5f;

    [Header("Debug")]
    [SerializeField] private bool showDebugLog = true;

    [Header("Property")]
    public InputHandler InputHandler => inputHandler;
    public MovementController MovementController => movementController;
    public JumpController JumpController => jumpController;
    public GroundDetector GroundDetector => groundDetector;
    public HighFallDetector HighFallDetector => highFallDetector;

    public PlayerAnimationController PlayerAnimationController => playerAnimationController;

    public CharacterController CharacterController => characterController;

    public float FallThreshold => fallThreshold;

    public float JumpStartY => jumpStartY;
    private float jumpStartY;

    public bool WasGrounded => wasGrounded;
    private bool wasGrounded;

    public bool ControllerGroundedAfterMove { get; private set; }

    private void Reset()
    {
        characterController = GetComponent<CharacterController>();
        inputHandler = GetComponent<InputHandler>();
        movementController = GetComponent<MovementController>();
        jumpController = GetComponent<JumpController>();
        groundDetector = GetComponent<GroundDetector>();
        highFallDetector = GetComponent<HighFallDetector>();
        playerAnimationController = GetComponent<PlayerAnimationController>();
        characterController = GetComponent<CharacterController>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void Awake()
    {
        if (characterController == null) characterController = GetComponent<CharacterController>();
        if (inputHandler == null) inputHandler = GetComponent<InputHandler>();
        if (movementController == null) movementController = GetComponent<MovementController>();
        if (jumpController == null) jumpController = GetComponent<JumpController>();
        if (groundDetector == null) groundDetector = GetComponent<GroundDetector>();
        if (highFallDetector == null) highFallDetector = GetComponent<HighFallDetector>();
        if (playerAnimationController == null) playerAnimationController = GetComponent<PlayerAnimationController>();
        if (characterController == null) characterController = GetComponent<CharacterController>();
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

        ControllerGroundedAfterMove = false;

        wasGrounded = groundDetector.IsGrounded;

        groundDetector.GroundCheck();
        jumpController.UpdateVertical(characterController.isGrounded, Time.deltaTime);

        ProcessMovement();

        playerStateMachine.UpdateState();

        //if (showDebugLog)
        //{
        //    Debug.Log($"Grounded : {groundDetector.IsGrounded} | Walkable : {groundDetector.IsWalkableSlope} | SlopeAngle : {groundDetector.SlopeAngle}");
        //}
    }

    private void ProcessMovement()
    {
        Vector3 moveDirection = movementController.GetMoveDirection(inputHandler.Movement, cameraTransform);

        bool hasMoveInput = moveDirection.sqrMagnitude > 0.01f;
        bool isRunPressed = inputHandler.IsRunPressed;

        Vector3 horizontalMove = movementController.GetHorizontalMove(moveDirection, Time.deltaTime, groundDetector.IsGrounded, isRunPressed);

        Vector3 finalMove = horizontalMove;
        finalMove.y = jumpController.VerticalVelocity;

        CollisionFlags flags = characterController.Move(finalMove * Time.deltaTime);

        ControllerGroundedAfterMove = (flags & CollisionFlags.Below) != 0;


        if (groundDetector.IsGrounded)
        {
            movementController.Rotate(transform, moveDirection, Time.deltaTime);
        }

        if (showDebugLog)
        {
            Debug.Log(
                $"TargetSpeed:{movementController.CurrentMoveSpeed:F2} | " +
                $"CurrentSpeed:{movementController.CurrentHorizontalSpeed:F2} | " +
                $"RunPressed:{isRunPressed} | " +
                $"HasMoveInput:{hasMoveInput}"
            );
        }

        if (!hasMoveInput) return;
        
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
            && highFallDetector != null
            && playerAnimationController != null
            && characterController != null
            && cameraTransform != null;
    }
}