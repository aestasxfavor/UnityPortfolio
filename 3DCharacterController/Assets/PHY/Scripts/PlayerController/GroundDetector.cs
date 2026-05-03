using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundLayer;

    [Header("Ground Distance")]
    [SerializeField] private float maxGroundCheckDistance = 1.0f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private float landingCheckDistance = 0.08f;

    [Header("Slope Setting")]
    [SerializeField] private float maxSlopeAngle = 45f;

    public Vector3 GroundNormal { get; private set; } = Vector3.up;
    public float SlopeAngle { get; private set; }

    public float DistanceToGround { get; private set; } = Mathf.Infinity;

    public bool IsGroundedDetected { get; private set; }
    public bool IsGrounded { get; private set; }

    public bool IsCloseEnoughToLand { get; private set; }
    public bool IsWalkableSlope { get; private set; }


    // Raycast 기반 지면 체크 로직
    public void GroundCheck()
    {
        if (groundCheckPoint == null)
        {
            ResetGroundInfo();
            return;
        }

        Vector3 origin = groundCheckPoint.position;
        Vector3 direction = Vector3.down;

        Debug.DrawRay(origin, direction * groundCheckDistance, Color.red);

        bool isHit = Physics.Raycast(
            origin,
            direction,
            out RaycastHit hit,
            maxGroundCheckDistance,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );

        IsGroundedDetected = isHit;

        if (isHit)
        {
            DistanceToGround = hit.distance;
            GroundNormal = hit.normal;
            SlopeAngle = Vector3.Angle(Vector3.up, hit.normal);
            IsWalkableSlope = SlopeAngle <= maxSlopeAngle;

            IsGrounded = DistanceToGround <= groundCheckDistance && IsWalkableSlope;
            IsCloseEnoughToLand = DistanceToGround <= landingCheckDistance;

            Debug.DrawRay(hit.point, GroundNormal * 0.7f, Color.blue);
        }
        else
        {
            ResetGroundInfo();
        }
    }

    private void ResetGroundInfo()
    {
        IsGroundedDetected = false;
        IsGrounded = false;
        IsCloseEnoughToLand = false;
        IsWalkableSlope = false;

        DistanceToGround = Mathf.Infinity;
        GroundNormal = Vector3.up;
        SlopeAngle = 0f;
    }

    // Raycast 범위를 씬에서 확인하기 위한 시각화 함수
    private void OnDrawGizmos()
    {
        if (groundCheckPoint == null) return;

        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(
            groundCheckPoint.position,
            groundCheckPoint.position + Vector3.down * groundCheckDistance
        );
    }
}