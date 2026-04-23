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

        bool isGrounded = playerController.GroundDetector.IsGrounded;
        bool hasMoveInput = playerController.InputHandler.Movement.sqrMagnitude > 0.01f;

        float ySpeed = playerController.JumpController.VerticalVelocity;
        float currentY = playerController.transform.position.y;
        float jumpStartY = playerController.JumpStartY;
        float fallDistance = jumpStartY - currentY;

        float fallThreshold = playerController.FallThreshold;
        float fallStartDistance = playerController.FallStartDistance;

        if (isGrounded)
        {
            if (hasMoveInput)
            {
                playerStateMachine.ChangeState(new PlayerMoveState(playerController, playerStateMachine));
            }
            else
            {
                playerStateMachine.ChangeState(new PlayerIdleState(playerController, playerStateMachine));
            }
        }

        if (ySpeed <= fallThreshold && fallDistance >= fallStartDistance)
        {
            playerStateMachine.ChangeState(new PlayerFallState(playerController, playerStateMachine));
            return;
        }


    }

    public override void Exit()
    {
        Debug.Log("Jump exit");
    }
}
