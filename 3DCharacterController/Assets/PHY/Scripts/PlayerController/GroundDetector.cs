using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 1.0f;

    [Header("Slope Setting")]
    [SerializeField] private float maxSlopeAngle = 45f;

    public Vector3 GroundNormal { get; private set; } = Vector3.up;
    public float SlopeAngle { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsWalkableSlope { get; private set; }

    // Raycast 기반 지면 체크 로직
    public void GroundCheck()
    {
        if (groundCheckPoint == null)
        {
            IsGrounded = false;
            GroundNormal = Vector3.up;
            SlopeAngle = 0f;
            IsWalkableSlope = false;
            Debug.LogWarning("GroundCheckPoint is null");
            return;
        }

        Vector3 origin = groundCheckPoint.position;
        Vector3 direction = Vector3.down;

        Debug.DrawRay(origin, direction * groundCheckDistance, Color.red);

        bool isHit = Physics.Raycast(
            origin,
            direction,
            out RaycastHit hit,
            groundCheckDistance,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );

        IsGrounded = isHit;

        if (isHit)
        {
            GroundNormal = hit.normal;
            SlopeAngle = Vector3.Angle(Vector3.up, hit.normal);
            IsWalkableSlope = SlopeAngle <= maxSlopeAngle;

            Debug.DrawRay(hit.point, GroundNormal * 0.7f, Color.blue);
        }
        else
        {
            GroundNormal = Vector3.up;
            SlopeAngle = 0f;
            IsWalkableSlope = false;
        }
    }

    // Raycast 범위를 씬에서 확인하기 위한 시각화 함수
    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null) return;

        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(
            groundCheckPoint.position,
            groundCheckPoint.position + Vector3.down * groundCheckDistance
        );
    }
}