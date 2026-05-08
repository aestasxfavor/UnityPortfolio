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

    public CharacterController CharacterController => characterController;
    public PlayerAnimationController PlayerAnimationController => playerAnimationController;
    public InputHandler InputHandler => inputHandler;
    public MovementController MovementController => movementController;
    public JumpController JumpController => jumpController;
    public GroundDetector GroundDetector => groundDetector;
    public HighFallDetector HighFallDetector => highFallDetector;

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
        if (playerStateMachine == null) playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void Start()
    {
        groundDetector.CheckGround();
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
        if (!IsReady()) return;

        ControllerGroundedAfterMove = false;

        wasGrounded = groundDetector.IsGrounded;

        groundDetector.CheckGround();
        jumpController.UpdateVertical(characterController.isGrounded, Time.deltaTime);

        ProcessMovement();

        playerStateMachine.UpdateState();
    }

    private void ProcessMovement()
    {
        Vector3 moveDirection = movementController.GetMoveDirection(inputHandler.Movement, cameraTransform);

        bool hasMoveInput = moveDirection.sqrMagnitude > 0.01f;
        bool isRunPressed = inputHandler.IsRunPressed;

        playerAnimationController.SetMoveInput(hasMoveInput);

        Vector3 horizontalMove = movementController.GetHorizontalMove(moveDirection, Time.deltaTime, 
            groundDetector.IsGrounded, isRunPressed);

        Vector3 finalMove = horizontalMove;
        finalMove.y = jumpController.VerticalVelocity;

        // CollisionFlags: Move 후 캐릭터 컨트롤러가 어떤 방향으로 충돌이 있었는지 알려주는 플래그
        CollisionFlags flags = characterController.Move(finalMove * Time.deltaTime);

        bool hitBelow = (flags & CollisionFlags.Below) != 0;

        groundDetector.CheckGround();

        ControllerGroundedAfterMove = hitBelow && groundDetector.IsGrounded;

        if (hasMoveInput && groundDetector.IsGrounded)
        {
            movementController.Rotate(transform, moveDirection, Time.deltaTime);
        }
    }

    public void SetJumpStartY(float value)
    {
        jumpStartY = value;
    }

    private bool IsReady()
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