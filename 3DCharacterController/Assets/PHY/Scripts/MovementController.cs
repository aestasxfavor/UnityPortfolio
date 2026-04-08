using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;

    public Vector3 GetMoveDirection(Vector2 movement, Transform cameraTransform)
    {
        if (movement.sqrMagnitude < 0.01f) return Vector3.zero;


        if (cameraTransform == null)
        {
            Vector3 fallbackDirection = new Vector3(movement.x, 0f, movement.y).normalized;
            //Debug.Log($"Camera 없음 | Input: {movement} | MoveDirection: {fallbackDirection}");
            return fallbackDirection;
        }

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * movement.y + cameraRight * movement.x;
        moveDirection = moveDirection.normalized;
        //Debug.Log($"Input: {movement} | MoveDirection: {moveDirection}");

        return moveDirection;
    }

    public void Move(Rigidbody rb, Vector3 moveDirection)
    {
        //target.position += moveDirection * moveSpeed * Time.deltaTime; => 기존 코드
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;
    }

    /// <summary>
    /// 경사면에서 방향키 조작하면 캐릭터가 빙그르르 도는 현상 발견함
    /// 5단계 작업인 이동 방향 기반 회전 구현에서 정밀하게 로직 구현할 예정
    /// </summary>
    /// <param name="target"></param>
    /// <param name="moveDirection"></param>
    public void Rotate(Transform target, Vector3 moveDirection)
    {
        if (moveDirection.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        target.rotation = Quaternion.Slerp(target.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}