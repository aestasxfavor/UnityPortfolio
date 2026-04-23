using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerController _playerController, PlayerStateMachine _playerStateMachine)
        : base(_playerController, _playerStateMachine) { }

    public override void Enter()
    {
        Debug.Log("idle enter");
    }

    public override void Update()
    {
        if (playerController.WasGrounded
            && !playerController.GroundDetector.IsGrounded
            && !playerController.JumpController.IsJumping
            && playerController.JumpController.VerticalVelocity <= playerController.FallThreshold)
        {
            playerStateMachine.ChangeState(new PlayerFallState(playerController, playerStateMachine));
            return;
        }

        bool canJump = playerController.InputHandler.JumpPressed &&
                       playerController.GroundDetector.IsGrounded &&
                       playerController.GroundDetector.IsWalkableSlope;

        if (canJump)
        {
            playerController.SetJumpStartY(playerController.transform.position.y);
            playerController.JumpController.TryJump(true);
            playerController.InputHandler.ResetJumpInput();

            playerStateMachine.ChangeState(new PlayerJumpState(playerController, playerStateMachine));
            return;
        }


        if (playerController.InputHandler.Movement.sqrMagnitude > 0.01f)
        {
            playerStateMachine.ChangeState(new PlayerMoveState(playerController, playerStateMachine));
            //Debug.Log("player ChangeState : MoveSlope");
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log("idle exit");
    }
}


