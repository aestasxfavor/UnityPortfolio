using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    private bool isLandingHandled;
    private bool hasStartedFallingAnimation;
    private bool startFallMotion;

    public PlayerFallState(PlayerController _playerController, PlayerStateMachine _playerStateMachine, bool _startFallMotion)
        : base(_playerController, _playerStateMachine)
    {
        startFallMotion = _startFallMotion;
    }

    public override void Enter()
    {
        isLandingHandled = false;
        hasStartedFallingAnimation = false;

        Debug.Log("fall enter");
        playerController.PlayerAnimationController.SetLanding(false);

        if(startFallMotion)
        {
            hasStartedFallingAnimation = true;
            playerController.PlayerAnimationController.SetFalling(true);
            Debug.Log("falling animation started");

        }

        playerController.HighFallDetector.StartFall(playerController.JumpController.VerticalVelocity);
    }

    public override void Update()
    {
        if (!playerController.GroundDetector.IsGrounded)
        {
            playerController.HighFallDetector.TickFall(Time.deltaTime, playerController.JumpController.VerticalVelocity);
            return;
        }

        HandleLanding();
    }

    private void HandleLanding()
    {
        if (isLandingHandled) return;

        isLandingHandled = true;

        bool wasHighFall = playerController.HighFallDetector.ConsumeHighFallResult();

        Debug.Log($"falling 상태에서 착지 감지 / wasHighFall {wasHighFall}");

        if (wasHighFall)
        {
            playerController.PlayerAnimationController.SetLanding(true);
            playerStateMachine.ChangeState(new PlayerLandState(playerController, playerStateMachine));
            return;
        }

        playerController.PlayerAnimationController.SetFalling(false);
        playerController.PlayerAnimationController.SetLanding(false);

        ReturnToGroundState();
    }

    private void ReturnToGroundState()
    {
        if (playerController.InputHandler.Movement.sqrMagnitude > 0.01f)
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
        Debug.Log("falling exit");
    }
}