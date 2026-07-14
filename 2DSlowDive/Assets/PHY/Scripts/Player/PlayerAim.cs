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

    private HarpoonUI harpoonUI;
    private PlayerCtrls ctrls;
    private SpriteRenderer harpoonSR;

    public Vector2 HarpoonDirection { get; private set; }

    private AimState lastAimState = AimState.Front;
    private Vector2 lastAimInput = Vector2.right;

    public bool IsHarpoonReady { get; private set; }
    private bool wantToFire;

    private HarpoonType currentHarpoonType = HarpoonType.Normal;
    public HarpoonType CurrentHarpoonType => currentHarpoonType;

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
        ctrls.Player.SwitchHarpoon.performed += OnSwitchHarpoon;
        ctrls.Player.Fire.performed += OnFire;
        
    }

    private void OnDisable()
    {
        ctrls.Player.Hold.performed -= OnHold;
        ctrls.Player.Hold.canceled -= OnRelease;
        ctrls.Player.SwitchHarpoon.performed -= OnSwitchHarpoon;
        ctrls.Player.Fire.performed -= OnFire;
        ctrls.Disable();
    }

    private void Start()
    {
        harpoonUI = FindFirstObjectByType<HarpoonUI>();
        harpoonUI?.UpdateUI();
    }

    private void Update()
    {
        if (!IsHarpoonReady)
            return;

        Vector2 rawInput = ctrls.Player.Move.ReadValue<Vector2>();

        if (rawInput.sqrMagnitude > 0.01f)
        {
            lastAimInput = rawInput.normalized;
            lastAimState = GetAimState(lastAimInput);
        }

        UpdateAimVisual(lastAimState);
    }

    private void OnHold(InputAction.CallbackContext _)
    {
        IsHarpoonReady = true;
        movePlayer.SetHarpoonReady(true);
        harpoon.gameObject.SetActive(true);
        wantToFire = false;

        animator.SetBool("IsHoldingHarpoon", true);
    }

    private void OnRelease(InputAction.CallbackContext _)
    {
        IsHarpoonReady = false;
        movePlayer.SetHarpoonReady(false);
        harpoon.gameObject.SetActive(false);
        wantToFire = false;

        animator.SetBool("IsHoldingHarpoon", false);
        animator.SetInteger("AimDir", 0);
    }

    private void OnSwitchHarpoon(InputAction.CallbackContext _)
    {
        if (!SaveManager.Instance.HasHarpoonUpgrade())
            return;

        currentHarpoonType = currentHarpoonType == HarpoonType.Normal
            ? HarpoonType.Upgrade
            : HarpoonType.Normal;

        harpoonFire.SetHarpoonType(currentHarpoonType);
        harpoonUI?.UpdateUI();
    }

    private void OnFire(InputAction.CallbackContext _)
    {
        if (!IsHarpoonReady)
            return;

        wantToFire = true;
    }

    public void OnHarpoonFire()
    {
        if (!wantToFire)
            return;

        harpoonFire.FireHarpoon(HarpoonDirection);
        wantToFire = false;
    }

    private AimState GetAimState(Vector2 input)
    {
        if (Mathf.Abs(input.x) < 0.5f)
            return AimState.Front;

        if (input.y > 0.5f)
            return AimState.Up;

        if (input.y < -0.5f)
            return AimState.Down;

        return AimState.Front;
    }

    private void UpdateAimVisual(AimState state)
    {
        bool facingRight = movePlayer.LastMoveDir.x >= 0f;

        animator.SetInteger("AimDir",
            state == AimState.Up ? 1 :
            state == AimState.Down ? 2 : 0);

        HarpoonVisual visual = GetVisual(state, facingRight);
        ApplyVisual(visual);
    }

    private HarpoonVisual GetVisual(AimState state, bool facingRight)
    {
        if (state == AimState.Up)
            return facingRight ? rightUp : leftUp;

        if (state == AimState.Down)
            return facingRight ? rightDown : leftDown;

        return facingRight ? rightFront : leftFront;
    }

    private void ApplyVisual(HarpoonVisual visual)
    {
        harpoon.localPosition = visual.localPos;
        harpoon.localRotation = Quaternion.Euler(0, 0, visual.rotationZ);
        harpoonSR.flipX = visual.flipX;
        HarpoonDirection = visual.direction.normalized;
    }
}