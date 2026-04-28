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

        if (playerController.InputHandler.JumpPressed)
        {
            Debug.Log(
                $"[Idle Jump Check] " +
                $"JumpPressed:{playerController.InputHandler.JumpPressed}, " +
                $"IsGrounded:{playerController.GroundDetector.IsGrounded}, " +
                $"IsWalkableSlope:{playerController.GroundDetector.IsWalkableSlope}, " +
                $"VerticalVelocity:{playerController.JumpController.VerticalVelocity}, " +
                $"IsJumping:{playerController.JumpController.IsJumping}"
            );
        }

        if (canJump)
        {
            //Debug.Log("idle -> jump 전이 1");

            playerController.SetJumpStartY(playerController.transform.position.y);
            //Debug.Log("idle -> jump 전이 2 : SetJumpStartY 완료");

            playerController.JumpController.TryJump(true);
            //Debug.Log("idle -> jump 전이 3 : TryJump 완료");

            playerController.InputHandler.ResetJumpInput();
            //Debug.Log("idle -> jump 전이 4 : ResetJumpInput 완료");

            playerStateMachine.ChangeState(new PlayerJumpState(playerController, playerStateMachine));
            //Debug.Log("idle -> jump 전이 5 : ChangeState 호출 완료");

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


