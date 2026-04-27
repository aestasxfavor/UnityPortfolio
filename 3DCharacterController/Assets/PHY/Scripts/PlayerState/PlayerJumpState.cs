using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerController _playerController, PlayerStateMachine _playerStateMachine)
        : base(_playerController, _playerStateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("jump enter");
    }

    public override void Update()
    {
        if (playerController.JumpController.VerticalVelocity < playerController.FallThreshold)
        {
            playerStateMachine.ChangeState(new PlayerFallState(playerController, playerStateMachine));
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log("jump exit");
    }
}