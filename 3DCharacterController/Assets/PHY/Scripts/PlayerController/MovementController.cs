using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 720f;

    [SerializeField] private float moveResponse = 10f;
    [SerializeField] private float stopResponse = 20f;

    [SerializeField] private float airControl = 0.5f;
    [SerializeField] private float airMoveResponse = 5f;
    [SerializeField] private float airStopResponse = 5f;

    private Vector3 currentHorizontalVelocity;
    public float MoveSpeed => moveSpeed;

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
    public Vector3 GetHorizontalMove(Vector3 moveDirection, float deltaTime, bool isGrounded)
    {
        bool hasMoveInput = moveDirection.sqrMagnitude > 0.01f;

        float speedMultiplier = isGrounded ? 1f : airControl;

        Vector3 targetVelocity = moveDirection * moveSpeed * speedMultiplier;
        float response;

        if (isGrounded)
        {
            response = hasMoveInput ? moveResponse : stopResponse;
        }
        else
        {
            response = hasMoveInput ? airMoveResponse : airStopResponse;
        }

        currentHorizontalVelocity = Vector3.MoveTowards(currentHorizontalVelocity, targetVelocity, response * deltaTime);


        return currentHorizontalVelocity;
    }

    /// <summary>
    /// 경사면에서 방향키 조작하면 캐릭터가 빙그르르 도는 현상 발견함
    /// Slope 함수를 사용하려고 했으나 게임 캐릭터 컨트롤러 구현 목표인 것을 감안하여 
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