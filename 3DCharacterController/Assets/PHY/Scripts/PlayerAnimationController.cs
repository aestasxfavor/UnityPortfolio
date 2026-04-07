using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GroundDetector groundDetector;

    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
        groundDetector = GetComponent<GroundDetector>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // rb.linearVelocity.y가 아닌 z를 사용할 것
        float speed = horizontalVelocity.magnitude;
        float ySpeed = rb.linearVelocity.y;
        bool isGrounded = groundDetector.IsGrounded;

        animator.SetFloat("Speed", speed);
        animator.SetFloat("YSpeed", ySpeed);
        animator.SetBool("IsGrounded", isGrounded);
    }
}
