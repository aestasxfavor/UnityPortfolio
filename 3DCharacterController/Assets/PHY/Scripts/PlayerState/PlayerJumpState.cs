using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerController _playerController, PlayerStateMachine _playerStateMachine) : base(_playerController, _playerStateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Jump enter");
    }

    public override void Update()
    {
        if (playerController.JumpController.VerticalVelocity <= 0f)
        {
            playerStateMachine.ChangeState(new PlayerFallState(playerController, playerStateMachine));
            //Debug.Log("player ChangeState: Fall");
            return;
        }

    }

    public override void Exit()
    {
        Debug.Log("Jump exit");
    }
}
