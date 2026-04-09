using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private MovementController movementController;
    [SerializeField] private JumpController jumpController;
    [SerializeField] private GroundDetector groundDetector;

    [SerializeField] private bool showDebugLog = true;

    // 인스펙터 자동 참조
    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        movementController = GetComponent<MovementController>();
        jumpController = GetComponent<JumpController>();
        groundDetector = GetComponent<GroundDetector>();
    }

    // 필수 컴포넌트 참조 보정
    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (inputHandler == null) inputHandler = GetComponent<InputHandler>();
        if (movementController == null) movementController = GetComponent<MovementController>();
        if (jumpController == null) jumpController = GetComponent<JumpController>();
        if (groundDetector == null) groundDetector = GetComponent<GroundDetector>();
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
            Debug.Log($"Move : {inputHandler.Movement} | Grounded : {groundDetector.IsGrounded}");
        }
    }

    // 이동 입력 처리 로직
    private void ProcessMovement()
    {
        Vector3 moveDirection = movementController.GetMoveDirection(inputHandler.Movement,cameraTransform);

        movementController.Move(rb, moveDirection);
        movementController.Rotate(transform, moveDirection);
    }

    // 점프 입력 처리 로직
    private void ProcessJump()
    {
        if (!inputHandler.JumpPressed) return;

        jumpController.Jump(groundDetector.IsGrounded);
        inputHandler.ResetJumpInput();
    }

    // 필수 참조 확인
    private bool CanRun()
    {
        return rb != null 
            && inputHandler != null 
            && movementController != null
            && jumpController != null 
            && groundDetector != null;
    }
}