using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerBaseState CurrentState { get; private set; }
    public void ChangeState(PlayerBaseState newState)
    {
        if (newState == null) return;

        if(CurrentState != null && CurrentState.GetType() == newState.GetType())
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
