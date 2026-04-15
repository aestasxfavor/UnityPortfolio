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
        if (!playerController.GroundDetector.IsGrounded) return;

        if(playerController.InputHandler.JumpPressed)
        {
            playerStateMachine.ChangeState(new PlayerJumpState(playerController, playerStateMachine));
            //Debug.Log("player ChangeState : Jump");
            return;
        }

        if(playerController.InputHandler.Movement.sqrMagnitude > 0.01f)
        {
            playerStateMachine.ChangeState(new PlayerMoveState(playerController, playerStateMachine));
            //Debug.Log("player ChangeState : Move");
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log("idle exit");
    }
}


