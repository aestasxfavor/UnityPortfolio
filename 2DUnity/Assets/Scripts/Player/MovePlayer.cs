using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
[RequireComponent(typeof(Rigidbody2D))]

public class MovePlayer : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerCtrls ctrls;

    [SerializeField] private SpriteRenderer playerSR;
    [SerializeField] private Transform playerVisual;

    // Todo: 2차 리팩때 SO로 분리하기
    [SerializeField] private float speed = 5f;

    public Vector2 LastMoveDir => lastMoveDir;
    public Vector2 InputDirection => inputDirection;
    private Vector2 lastMoveDir = Vector2.right;  // 최근 방향 기억용
    private Vector2 inputDirection;

    public bool IsHarpoonReady { get; set; }

    private void Awake()
    {
        ctrls = new PlayerCtrls();
        rb = GetComponent<Rigidbody2D>();

    }

    private void OnEnable() => ctrls.Enable();



    private void OnDisable() => ctrls.Disable();


    private void FixedUpdate()
    {
        if (IsHarpoonReady)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        inputDirection = ctrls.Player.Move.ReadValue<Vector2>();
        rb.linearVelocity = inputDirection * speed;

        if (Mathf.Abs(inputDirection.x) > 0.01f)
        {
            lastMoveDir = new Vector2(Mathf.Sign(inputDirection.x), 0f);
        }

    }

    private void LateUpdate()
    {
        if (IsHarpoonReady) return;
        VisualDirection();
    }

    private void VisualDirection()
    {
        // 기본 Idle 상태
        if (inputDirection == Vector2.zero)
        {
            playerVisual.rotation = Quaternion.identity;
            playerSR.flipY = false;

            playerSR.flipX = (lastMoveDir.x < 0f);
            return;
        }

        // 오른쪽 이동
        if (inputDirection.x > 0.01f)
        {
            playerVisual.rotation = Quaternion.Euler(0, 0, -90f);
            playerSR.flipX = false;
            playerSR.flipY = false;
            return;
        }

        // 왼쪽 이동
        if (inputDirection.x < -0.01f)
        {
            playerVisual.rotation = Quaternion.Euler(0, 0, 90f);
            playerSR.flipX = true;
            playerSR.flipY = false;
            return;
        }

        // 상하 이동
        if (Mathf.Abs(inputDirection.y) > 0.01f)
        {
            playerVisual.rotation = Quaternion.identity;
            playerSR.flipX = (lastMoveDir.x < 0f);
            playerSR.flipY = false;
        }
    }

}