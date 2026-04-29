using UnityEngine;

public class PlayerLandState : PlayerBaseState
{
    private float landingTimer;
    private float landingDuration = 0.25f;
    public PlayerLandState(PlayerController _playerController, PlayerStateMachine _playerStateMachine)
        : base(_playerController, _playerStateMachine)
    {
    }

    public override void Enter()
    {
        landingTimer = 0f;
        Debug.Log("landing enter");
    }

    public override void Update()
    {
        landingTimer += Time.deltaTime;
        if (landingTimer < landingDuration) return;
        
        if(playerController.InputHandler.Movement.sqrMagnitude > 0.01f)
        {
            playerStateMachine.ChangeState(new PlayerMoveState(playerController, playerStateMachine));
        }
        else
        {
            playerStateMachine.ChangeState(new PlayerIdleState(playerController, playerStateMachine));
        }
    }

    public override void Exit()
    {
        Debug.Log("landing exit");
    }
}