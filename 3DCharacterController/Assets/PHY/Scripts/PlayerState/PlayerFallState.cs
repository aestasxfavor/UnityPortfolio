using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    private bool isLandingHandled;
    private bool hasStartedFallingAnimation;
    private bool startFallMotion;

    private float fallTimer;
    private float minYSpeed;
   

    private const float HighFallMinTime = 0.35f;
    private const float HighFallMinYSpeed = -4f;

    public PlayerFallState(PlayerController _playerController, PlayerStateMachine _playerStateMachine, bool _startFallMotion)
        : base(_playerController, _playerStateMachine)
    {
        startFallMotion = _startFallMotion;
    }

    public override void Enter()
    {
        isLandingHandled = false;
        hasStartedFallingAnimation = false;

        fallTimer = 0f;
        minYSpeed = playerController.JumpController.VerticalVelocity;

        playerController.PlayerAnimationController.SetLanding(false);
        playerController.PlayerAnimationController.SetFalling(false);

        Debug.Log("fall enter");

        playerController.HighFallDetector.StartFall(playerController.JumpController.VerticalVelocity);
    }

    public override void Update()
    {
        fallTimer += Time.deltaTime;
        minYSpeed = Mathf.Min(minYSpeed, playerController.JumpController.VerticalVelocity);

        playerController.HighFallDetector.TickFall(Time.deltaTime, playerController.JumpController.VerticalVelocity);

        if (!hasStartedFallingAnimation && IsHighFallNow())
        {
            StartFallingAnimation();
        }

        if (fallTimer < 0.05f) return;

        // CharacterController.Move 이후 바닥 충돌이 있었으면 착지 처리
        if (playerController.ControllerGroundedAfterMove)
        {
            HandleLanding();
            return;
        }
    }

    private bool IsHighFallNow()
    {
        return fallTimer >= HighFallMinTime && minYSpeed <= HighFallMinYSpeed;
    }

    private void StartFallingAnimation()
    {
        hasStartedFallingAnimation = true;
     
        playerController.PlayerAnimationController.SetFalling(true);
    }

    private void HandleLanding()
    {
        if (isLandingHandled) return;

        isLandingHandled = true;

        bool wasHighFall = playerController.HighFallDetector.ConsumeHighFallResult();
        bool shouldPlayLandingMotion = wasHighFall || hasStartedFallingAnimation;

        playerController.PlayerAnimationController.SetFalling(false);

        if (shouldPlayLandingMotion)
        {
            playerStateMachine.ChangeState(new PlayerLandState(playerController, playerStateMachine));

            return;
        }

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