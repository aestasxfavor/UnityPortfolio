using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerBaseState CurrentState { get; private set; }
    public void ChangeState(PlayerBaseState newState)
    {
        if (newState == null) return;

        // 같은 상태로 반복 전환되는 것을 방지
        // Enter / Exit가 매 프레임 다시 호출되는 문제를 막기 위한 방어 코드
        if (CurrentState != null && CurrentState.GetType() == newState.GetType())
        {
            return;
        }

        Debug.Log($"ChangeState: {CurrentState?.GetType().Name} -> {newState.GetType().Name}");

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }

    public void UpdateState()
    {
        CurrentState?.Update();
    }
}
