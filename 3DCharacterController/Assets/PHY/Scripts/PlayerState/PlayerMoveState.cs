using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerController _playerController, PlayerStateMachine _playerStateMachine) : base(_playerController, _playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerController.PlayerAnimationController.SetFalling(false);
        playerController.PlayerAnimationController.SetLanding(false);
        playerController.PlayerAnimationController.SetMoveInput(true);
        Debug.Log("move enter");
    }

    public override void Update()
    {
        if (ShouldFall())
        {
            playerStateMachine.ChangeState(new PlayerFallState(playerController, playerStateMachine, true));
            return;
        }


        bool canJump = playerController.InputHandler.JumpPressed &&
                       playerController.GroundDetector.IsGrounded &&
                       playerController.GroundDetector.IsWalkableSlope;

        if (canJump)
        {
            Debug.Log("move -> jump ÀüÀÌ");
            playerController.SetJumpStartY(playerController.transform.position.y);
            playerController.JumpController.TryJump(true);
            playerController.InputHandler.ResetJumpInput();

            playerStateMachine.ChangeState(new PlayerJumpState(playerController, playerStateMachine));
            return;
        }

        if (playerController.InputHandler.Movement.sqrMagnitude <= 0.01f)
        {
            playerStateMachine.ChangeState(new PlayerIdleState(playerController, playerStateMachine));
            return;
        }

    }

    private bool ShouldFall()
    {
        return playerController.WasGrounded
            && !playerController.GroundDetector.IsGrounded
            && !playerController.JumpController.IsJumping
            && playerController.JumpController.VerticalVelocity <= playerController.FallThreshold;
    }

    public override void Exit()
    {
        Debug.Log("move exit");
    }
}
