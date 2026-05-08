using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    private bool LandingDone;
    private bool hasPlayedFalling;
    private bool startFallMotion;

    private float fallTimer;
    private float lowYSpeed;

    // 높은 낙하 판정용
    private const float HighFallTime = 0.35f;
    private const float HighFallSpeed = -4f;    // 원래는 -4, 현재 테스트중 => 다시 봐야할 거 같음 일단 보류

    // Falling모션 시작용
    private float FallMotionDelay = 0.12f;
    private float FallMotionSpeed = -2.5f;
    public PlayerFallState(PlayerController _playerController, PlayerStateMachine _playerStateMachine, bool _startFallMotion)
        : base(_playerController, _playerStateMachine)
    {
        startFallMotion = _startFallMotion;
    }

    public override void Enter()
    {
        LandingDone = false;
        hasPlayedFalling = false;

        fallTimer = 0f;
        lowYSpeed = playerController.JumpController.VerticalVelocity;

        playerController.PlayerAnimationController.SetLanding(false);
        playerController.PlayerAnimationController.SetFalling(false);

        Debug.Log("fall enter");

        // 이걸 키면 falling 모션이 바로 나와서 좋긴한데 박스나 낮은 단차에도 나와버림
        // 그렇다고 끄면 낙하할 때 모션이 제대로 안나오고 씹히는 경우가 많음 ㅈ같네 진짜
        // 분기를 대체 어떻게 둬야하는걸까
        //if (startFallMotion)
        //{
        //    StartFallingAnimation();
        //}

        playerController.HighFallDetector.StartFall(playerController.JumpController.VerticalVelocity);
    }

    public override void Update()
    {
        fallTimer += Time.deltaTime;
        lowYSpeed = Mathf.Min(lowYSpeed, playerController.JumpController.VerticalVelocity);

        playerController.HighFallDetector.CheckHighFall(Time.deltaTime, playerController.JumpController.VerticalVelocity);

        if (!hasPlayedFalling && ShouldStartFallingMotion())
        {
            StartFallingAnimation();
        }

        if (fallTimer < 0.05f) return;

        // CharacterController.Move 이후 바닥 충돌이 있었으면 착지 처리
        if (playerController.ControllerGroundedAfterMove)
        {
            Land();
            return;
        }
    }

    private bool IsHighFall()
    {
        return fallTimer >= HighFallTime && lowYSpeed <= HighFallSpeed;
    }

    private void StartFallingAnimation()
    {
        hasPlayedFalling = true;

        playerController.PlayerAnimationController.SetFalling(true);
    }

    /// <summary>
    /// 1. FallState 들어오자마자는 기다림
    ///  2. 일정 시간 이상 공중에 있었는지 확인
    ///  3. 아직 땅에 너무 가깝지 않은지 확인
    ///  4. Y속도가 충분히 아래로 떨어졌는지 확인
    /// </summary>
    /// <returns></returns>
    private bool ShouldStartFallingMotion()
    {
        if (fallTimer < FallMotionDelay) return false;

        if (playerController.GroundDetector.HasGroundHit) return false;

        return lowYSpeed <= FallMotionSpeed;
    }

    private void Land()
    {
        if (LandingDone) return;

        LandingDone = true;

        bool wasHighFall = playerController.HighFallDetector.GetResultAndReset();
        bool shouldPlayLandingMotion = wasHighFall || hasPlayedFalling;

        playerController.PlayerAnimationController.SetFalling(false);

        if (shouldPlayLandingMotion)
        {
            playerStateMachine.ChangeState(new PlayerLandState(playerController, playerStateMachine));

            return;
        }

        playerController.PlayerAnimationController.SetLanding(false);
        ChangeToIdleOrMove();
    }

    private void ChangeToIdleOrMove()
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