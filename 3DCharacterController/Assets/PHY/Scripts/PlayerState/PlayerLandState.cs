using UnityEngine;

public class PlayerLandState : PlayerBaseState
{
    private float landingTimer;
    private float idlelandingDuration = 0.35f;
    private float moveLandingDuration = 0.2f;

    public PlayerLandState(PlayerController _playerController, PlayerStateMachine _playerStateMachine)
        : base(_playerController, _playerStateMachine)
    {
    }

    public override void Enter()
    {
        landingTimer = 0f;

        playerController.PlayerAnimationController.SetFalling(false);
        playerController.PlayerAnimationController.SetLanding(true);

        Debug.Log("landing enter");
    }

    public override void Update()
    {
        landingTimer += Time.deltaTime;

        bool hasMoveInput = playerController.InputHandler.Movement.sqrMagnitude > 0.01f;

        playerController.PlayerAnimationController.SetMoveInput(hasMoveInput);

        float targetDuration = hasMoveInput ? moveLandingDuration : idlelandingDuration;

        if (landingTimer < targetDuration) return;
        
        if(hasMoveInput)
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
        playerController.PlayerAnimationController.SetLanding(false);
        playerController.PlayerAnimationController.SetFalling(false);

        Debug.Log("landing exit");
    }
}