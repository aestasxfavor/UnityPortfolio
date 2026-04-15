using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerController _playerController, PlayerStateMachine _playerStateMachine) : base(_playerController, _playerStateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("move enter");
    }

    public override void Update()
    {
        if (!playerController.GroundDetector.IsGrounded) return;

        if (playerController.InputHandler.Movement.sqrMagnitude <= 0.01f)
        {
            playerStateMachine.ChangeState(new PlayerIdleState(playerController, playerStateMachine));
            //Debug.Log("player ChangeState: Idle");
            return;
        }

    }

    public override void Exit()
    {
        Debug.Log("move exit");
    }
}
