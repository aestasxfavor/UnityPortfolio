using UnityEngine;
using UnityEngine.InputSystem;

public enum AimState
{
    Front,
    Up,
    Down
}

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform harpoon;
    [SerializeField] private MovePlayer movePlayer;
    [SerializeField] private HarpoonFire harpoonFire;

    private PlayerCtrls ctrls;
    private SpriteRenderer harpoonSR;

    public bool IsHarpoonReady { get; private set; }
    public Vector2 HarpoonDirection { get; private set; }

    #region Visual Data
    [System.Serializable]
    private struct HarpoonVisual
    {
        public Vector3 localPos;
        public float rotationZ;
        public bool flipX;
        public Vector2 direction;
    }

    [Header("Right")]
    [SerializeField] private HarpoonVisual rightFront;
    [SerializeField] private HarpoonVisual rightUp;
    [SerializeField] private HarpoonVisual rightDown;

    [Header("Left")]
    [SerializeField] private HarpoonVisual leftFront;
    [SerializeField] private HarpoonVisual leftUp;
    [SerializeField] private HarpoonVisual leftDown;
    #endregion

    private void Awake()
    {
        ctrls = new PlayerCtrls();
        harpoonSR = harpoon.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        ctrls.Enable();
        ctrls.Player.Hold.performed += OnHold;
        ctrls.Player.Hold.canceled += OnRelease;
    }

    private void OnDisable()
    {
        ctrls.Player.Hold.performed -= OnHold;
        ctrls.Player.Hold.canceled -= OnRelease;
        ctrls.Disable();
    }

    private void OnHold(InputAction.CallbackContext _)
    {
        IsHarpoonReady = true;
        harpoon.gameObject.SetActive(true);
    }

    private void OnRelease(InputAction.CallbackContext _)
    {
        IsHarpoonReady = false;
        animator.SetInteger("AimDir", 0);
        harpoon.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!IsHarpoonReady)
            return;

        Vector2 input = ctrls.Player.Move.ReadValue<Vector2>();
        AimState state = GetAimState(input);

        UpdateAimVisual(state);

        if (Keyboard.current.shiftKey.wasPressedThisFrame)
        {
            harpoonFire.FireHarpoon(HarpoonDirection);
        }
    }

    private AimState GetAimState(Vector2 input)
    {
        if (input.y > 0.5f) return AimState.Up;
        if (input.y < -0.5f) return AimState.Down;
        return AimState.Front;
    }

    private void UpdateAimVisual(AimState state)
    {
        Vector2 baseDir = movePlayer.LastMoveDir;
        bool facingRight = baseDir.x >= 0f;

        animator.SetInteger("AimDir",
            state == AimState.Up ? 1 :
            state == AimState.Down ? 2 : 0);

        HarpoonVisual visual = GetVisual(state, facingRight);
        
        ApplyVisual(visual);

    }

    private HarpoonVisual GetVisual(AimState state, bool facingRight)
    {
        if (state == AimState.Up)
        {
            return facingRight ? rightUp : leftUp;
        }
        else if (state == AimState.Down)
        {
            return facingRight ? rightDown : leftDown;
        }
        else
        {
            return facingRight ? rightFront : leftFront;
        }
    }

    private void ApplyVisual(HarpoonVisual visual)
    {
        harpoon.localPosition = visual.localPos;
        harpoon.localRotation = Quaternion.Euler(0, 0, visual.rotationZ);
        harpoonSR.flipX = visual.flipX;
        HarpoonDirection = visual.direction.normalized;
    }
}
