using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    private bool isLanded;
    private float fallTime;

    private const float landingfallTime = 0.25f;
    public PlayerFallState(PlayerController _playerController, PlayerStateMachine _playerStateMachine)
        : base(_playerController, _playerStateMachine)
    {
    }

    public override void Enter()
    {
        isLanded = false;
        fallTime = 0f;
        Debug.Log("fall enter");
    }

    public override void Update()
    {
        fallTime += Time.deltaTime;

        if (playerController.GroundDetector.IsGrounded)
        {
            if(!isLanded)
            {
                isLanded = true;
                Debug.Log("fall 상태에서 착지 감지");
            }

            if(fallTime >= landingfallTime)
            {
                playerStateMachine.ChangeState(new PlayerLandingState(playerController, playerStateMachine));
                return;
            }

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