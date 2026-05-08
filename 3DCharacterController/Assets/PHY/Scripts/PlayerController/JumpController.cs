using UnityEngine;

public class JumpController : MonoBehaviour
{
    [SerializeField] private float jumpPower = 6f;
    [SerializeField] private float gravityScale = 3.5f;
    [SerializeField] private float maxFallSpeed = 20f;
    [SerializeField] private float groundStickVelocity = -0.5f;

    public float VerticalVelocity { get; private set; }
    public bool IsJumping { get; private set; }

    private bool hasLeftGround;

    public void TryJump(bool isGrounded)
    {
        if (!isGrounded) return;

        VerticalVelocity = jumpPower;
        IsJumping = true;
        hasLeftGround = false;
    }

    public void UpdateVertical(bool isGrounded, float deltaTime)
    {
        // 점프 중이고 실제로 땅에서 떨어진 프레임을 한 번이라도 겪었는지 기록
        if (IsJumping && !isGrounded)
        {
            hasLeftGround = true;
        }

        // 점프 중이 아니고 땅에 붙어 있으면 바닥 고정 속도만 유지
        if (!IsJumping && isGrounded)
        {
            if (VerticalVelocity < 0f)
            {
                VerticalVelocity = groundStickVelocity;
            }

            return;
        }

        // 점프 상승/하강 중에는 grounded가 잠깐 true여도 중력은 계속 적용
        ApplyGravity(deltaTime);

        LimitFallSpeed();

        // 점프 후 실제로 공중에 나간 적이 있고, 다시 땅에 닿았을 때만 착지 처리
        if (IsJumping && hasLeftGround && isGrounded && VerticalVelocity <= 0f)
        {
            EndJump();
        }
    }

    private void ApplyGravity(float deltaTime)
    {
        VerticalVelocity += Physics.gravity.y * gravityScale * deltaTime;
    }

    private void LimitFallSpeed()
    {
        if (VerticalVelocity < -maxFallSpeed)
        {
            VerticalVelocity = -maxFallSpeed;
        }
    }

    private void EndJump()
    {
        IsJumping = false;
        hasLeftGround = false;
        VerticalVelocity = groundStickVelocity;
    }

    
}