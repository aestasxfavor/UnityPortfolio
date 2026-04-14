using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 720f;

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

    // Rigidbody에 수평 이동만 적용하는 로직
    public void Move(Rigidbody rb, Vector3 moveDirection)
    {
        Vector3 velocity = rb.linearVelocity;
        Vector3 horizonVelocity = moveDirection * moveSpeed;

        velocity.x = horizonVelocity.x;
        velocity.z = horizonVelocity.z;

        rb.linearVelocity = velocity;
    }

    /// <summary>
    /// 경사면에서 방향키 조작하면 캐릭터가 빙그르르 도는 현상 발견함
    /// 5단계 작업인 이동 방향 기반 회전 구현에서 정밀하게 로직 구현할 예정 -> 7단계 경사면 대응에서 해야하는건가? 
    /// Slope 함수를 사용하려고 했으나 게임 캐릭터 컨트롤러 구현 목표인 것을 감안하여 
    /// 좀 더 게임 느낌이 나도록하기위해RotateTowards 함수를 사용함
    /// </summary>
    /// <param name="target"></param>
    /// <param name="moveDirection"></param>

    // 이동 방향 기준 회전 로직
    public void Rotate(Transform target, Vector3 moveDirection, float deltaTime)
    {
        if (moveDirection.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        //target.rotation = Quaternion.Slerp(target.rotation, targetRotation, rotationSpeed * deltaTime);
        target.rotation = Quaternion.RotateTowards(target.rotation, targetRotation, rotationSpeed * deltaTime);
    }
}