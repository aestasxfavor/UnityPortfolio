using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 7f;

    [SerializeField] private float rotationSpeed = 1080f;

    [SerializeField] private float moveResponse = 10f;
    //[SerializeField] private float stopResponse = 20f;      // 입력을 멈췄을 때 수평 속도를 빠르게 줄이기 위한 감속 반응값
    [SerializeField] private float runToWalkResponse = 4f;

    [SerializeField] private float airControl = 0.5f;
    [SerializeField] private float airMoveResponse = 5f;
    [SerializeField] private float airStopResponse = 5f;

    private Vector3 currentHorizontalVelocity;

    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;
    public float CurrentHorizontalSpeed => currentHorizontalVelocity.magnitude;
    public float CurrentMoveSpeed { get; private set; }

    public bool IsRunning { get; private set; }

    // 카메라 기준 방향 이동 계산 로직
    public Vector3 GetMoveDirection(Vector2 movement, Transform cameraTransform)
    {
        if (movement.sqrMagnitude < 0.01f) return Vector3.zero;

        // 카메라 없으면 입력값 기준 기본 방향 사용
        if (cameraTransform == null)
        {
            Vector3 fallbackDirection = new Vector3(movement.x, 0f, movement.y).normalized;
            return fallbackDirection;
        }

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // 수평 이동만 반영하기 위한 y축 제거 로직
        cameraForward.y = 0f;
        cameraRight.y = 0f;



        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * movement.y + cameraRight * movement.x;
        moveDirection = moveDirection.normalized;

        return moveDirection;
    }

    // 캐릭터 컨트롤러 기반 이동 로직
    public Vector3 GetHorizontalMove(Vector3 moveDirection, float deltaTime, bool isGrounded, bool isRunPressed)
    {
        bool hasMoveInput = moveDirection.sqrMagnitude > 0.01f;

        IsRunning = hasMoveInput && isRunPressed;

        CurrentMoveSpeed = hasMoveInput ? (IsRunning ? runSpeed : walkSpeed) : 0f;

        // 지상에서 입력이 없을 때는 수평 속도를 즉시 제거한다.
        // 감속 보간만 사용할 경우 키를 뗀 뒤 바닥에서 미끄러짐이 강해
        // 지상 정지는 조작감을 위해 즉시 정지 방식으로 처리한다.
        if (isGrounded && !hasMoveInput)
        {
            currentHorizontalVelocity = Vector3.zero;
            return Vector3.zero;
        }

        float speedMultiplier = isGrounded ? 1f : airControl;

        Vector3 targetVelocity = moveDirection * CurrentMoveSpeed * speedMultiplier;

        float response;

        if (isGrounded)
        {
            float currentSpeed = currentHorizontalVelocity.magnitude;
            float targetSpeed = targetVelocity.magnitude;

            bool isSlowingDown = currentSpeed > targetSpeed;

            response = isSlowingDown ? runToWalkResponse : moveResponse;
        }
        else
        {
            response = hasMoveInput ? airMoveResponse : airStopResponse;
        }

        currentHorizontalVelocity = Vector3.MoveTowards(
            currentHorizontalVelocity,
            targetVelocity,
            response * deltaTime
        );

        return currentHorizontalVelocity;
    }

    /// <summary>
    /// 경사면에서 방향키 조작하면 캐릭터가 빙그르르 도는 현상 발견함
    /// Slerp 함수를 사용하려고 했으나 게임 캐릭터 컨트롤러 구현 목표인 것을 감안하여 
    /// 좀 더 게임 느낌이 나도록하기위해RotateTowards 함수를 사용함
    /// </summary>
    /// <param name="target"></param>
    /// <param name="moveDirection"></param>

    // 이동 방향 기준 회전 로직
    public void Rotate(Transform target, Vector3 moveDirection, float deltaTime)
    {
        Vector3 flatDirection = new Vector3(moveDirection.x, 0f, moveDirection.z);

        if (flatDirection.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
        target.rotation = Quaternion.RotateTowards(target.rotation, targetRotation, rotationSpeed * deltaTime);


    }
}