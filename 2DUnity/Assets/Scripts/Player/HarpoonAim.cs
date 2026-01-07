using UnityEngine;

public class HarpoonAim : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerRenderer;
    [SerializeField] private Transform harpoon;
    [SerializeField] private MovePlayer movePlayer;

    public Vector2 HarpoonDirection { get; private set; }

    /// <summary>
    /// 외부(PlayerAim 등)에서 호출
    /// </summary>
    public void UpdateAim(Vector2 aimDir)
    {
        HarpoonDirection = aimDir.normalized;

        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;

        animator.SetFloat("HarpoonAngle", angle);

        bool isLeft = aimDir.x < 0f;
        playerRenderer.flipX = isLeft;

        if (!isLeft)
            harpoon.localRotation = Quaternion.Euler(0, 0, angle);
        else
            harpoon.localRotation = Quaternion.Euler(0, 180, -angle);
    }

    public void SetAimVisual(bool aiming)
    {
        animator.SetBool("IsHoldingHarpoon", aiming);
        harpoon.gameObject.SetActive(aiming);
    }
}
