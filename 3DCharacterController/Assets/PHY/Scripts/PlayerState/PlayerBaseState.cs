using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerController playerController;
    protected PlayerStateMachine playerStateMachine;

    protected PlayerBaseState(PlayerController _playerController, PlayerStateMachine _playerStateMachine)
    {
        playerController = _playerController;
        playerStateMachine = _playerStateMachine;
    }

    public abstract void Update();

    public virtual void Enter() { }
    public virtual void Exit() { }
}
