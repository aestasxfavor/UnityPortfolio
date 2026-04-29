using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private bool hasLeftGround;

    public PlayerJumpState(PlayerController _playerController, PlayerStateMachine _playerStateMachine)
        : base(_playerController, _playerStateMachine)
    {
    }

    public override void Enter()
    {
        hasLeftGround = false;
        Debug.Log("jump enter");
    }

    public override void Update()
    {
        if (!playerController.GroundDetector.IsGrounded)
        {
            hasLeftGround = true;
        }

        if (hasLeftGround && playerController.GroundDetector.IsGrounded)
        {
            if (playerController.InputHandler.Movement.sqrMagnitude > 0.01f)
            {
                playerStateMachine.ChangeState(new PlayerMoveState(playerController, playerStateMachine));
            }
            else
            {
                playerStateMachine.ChangeState(new PlayerIdleState(playerController, playerStateMachine));
            }

            return;
        }
    }

    public override void Exit()
    {
        Debug.Log("jump exit");
    }
}