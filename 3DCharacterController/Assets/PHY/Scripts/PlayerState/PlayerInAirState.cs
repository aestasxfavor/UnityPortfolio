using UnityEngine;

public class PlayerInAirState : PlayerBaseState
{
    public PlayerInAirState(PlayerController _playerController, PlayerStateMachine _playerStateMachine)
        : base(_playerController, _playerStateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("inAir enter");
    }

    public override void Update()
    {
        if (playerController.GroundDetector.IsGrounded)
        {
            Debug.Log("InAir 상태에서 착지 감지");

            playerStateMachine.ChangeState(new PlayerLandState(playerController, playerStateMachine));
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log("inAir exit");
    }
}