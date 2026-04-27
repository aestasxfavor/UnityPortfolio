using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerController _playerController, PlayerStateMachine _playerStateMachine)
        : base(_playerController, _playerStateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("fall enter");
    }

    public override void Update()
    {
        if (playerController.GroundDetector.IsGrounded)
        {
            if (playerController.InputHandler.Movement.sqrMagnitude > 0.01f)
            {
                playerStateMachine.ChangeState(new PlayerMoveState(playerController, playerStateMachine));
                return;
            }

            playerStateMachine.ChangeState(new PlayerIdleState(playerController, playerStateMachine));
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log("fall exit");
    }
}