using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerBaseState CurrentState { get; private set; }
    public void ChangeState(PlayerBaseState newState)
    {
        if (newState == null) return;

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }

    private void Update()
    {
        CurrentState?.Update();
    }
}
