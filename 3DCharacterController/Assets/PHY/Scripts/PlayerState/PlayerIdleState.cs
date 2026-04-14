using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerController _playerController, PlayerStateMachine _playerStateMachine)
        : base(_playerController, _playerStateMachine) { }

    public override void Enter()
    {
        Debug.Log("PlayerIdleState Enter");
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        Debug.Log("PlayerIdleState Exit");
    }
}


