using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 Movement { get; private set; }
    public bool JumpPressed { get; private set; }

    // 이동 입력값 저장 로직
    public void OnMove(InputValue value)
    {
        Movement = value.Get<Vector2>();
        Debug.Log($"[InputHandler] Movement Input : {Movement}");
    }

    // 점프 입력 상태 저장 로직
    public void OnJump()
    {
        JumpPressed = true;
    }

    // TryJump 입력 초기화 로직
    public void ResetJumpInput()
    {
        JumpPressed = false;
    }
}
