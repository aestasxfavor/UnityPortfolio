using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GroundDetector groundDetector;
    [SerializeField] private JumpController jumpController;

    private void Reset()
    {
        characterController = GetComponent<CharacterController>();
        groundDetector = GetComponent<GroundDetector>();
        jumpController = GetComponent<JumpController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (characterController == null || groundDetector == null || jumpController == null || animator == null) return;

        Vector3 horizontalVelocity = new Vector3(characterController.velocity.x, 0f, characterController.velocity.z);
        float speed = horizontalVelocity.magnitude;
        float ySpeed = jumpController.VerticalVelocity;
        bool isGrounded = groundDetector.IsGrounded;

        animator.SetFloat("Speed", speed);
        animator.SetFloat("YSpeed", ySpeed);
        animator.SetBool("IsGrounded", isGrounded);
    }
}
