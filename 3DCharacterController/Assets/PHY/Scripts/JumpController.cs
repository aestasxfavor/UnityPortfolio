using UnityEngine;

public class JumpController : MonoBehaviour
{
    [SerializeField] private float jumpPower = 6f;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float maxFallSpeed = 20f;
    [SerializeField] private float groundY = -2f;

    public float VerticalVelocity { get; private set; }

    // 점프 처리 로직
    public void Jump(bool isGrounded)
    {
        if (!isGrounded) return;

        VerticalVelocity = jumpPower;
    }

    // 중력 적용 로직
    public void ApplyGravity(float deltaTime)
    {
        VerticalVelocity += Physics.gravity.y * gravityScale * deltaTime;
    }

    // 낙하 속도 제어 로직
    public void LimitFallSpeed()
    {
        if (VerticalVelocity < -maxFallSpeed)
        {
            VerticalVelocity = -maxFallSpeed;
        }
    }

    // 착지 후 수직값 정리 로직
    public void ResetVertical(bool isGrounded)
    {
        if (isGrounded && VerticalVelocity < 0f)
        {
            VerticalVelocity = groundY;
        }
    }

    public void UpdateVertical(bool isGrounded, float deltaTime)
    {
        ResetVertical(isGrounded);

        if (!isGrounded)
        {
            ApplyGravity(deltaTime);
            LimitFallSpeed();
        }
    }
}
