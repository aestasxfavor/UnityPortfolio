using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckDistance = 1.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float slopeLimit = 45f;

    public bool IsGrounded { get; private set; }
    public Vector3 GroundNormal { get; private set; } = Vector3.up;
    public float SlopeAngle { get; private set; }

    // Raycast 기반 지면 체크 로직
    public void GroundCheck()
    {
        if (groundCheckPoint == null)
        {
            IsGrounded = false;
            GroundNormal = Vector3.up;
            SlopeAngle = 0f;
            Debug.LogWarning("GroundCheckPoint is null");
            return;
        }

        Vector3 origin = groundCheckPoint.position;
        float distance = groundCheckDistance;

        Debug.DrawRay(origin, Vector3.down * distance, Color.red);

        bool isHit = Physics.Raycast(
            origin,
            Vector3.down,
            out RaycastHit hit,
            distance,
            groundLayer,
            QueryTriggerInteraction.Ignore
        );

        if (isHit)
        {
            GroundNormal = hit.normal;
            SlopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            IsGrounded = SlopeAngle <= slopeLimit;

            //Debug.Log(
            //    $"HIT | obj:{hit.collider.name} | layer:{LayerMask.LayerToName(hit.collider.gameObject.layer)} | hitDist:{hit.distance:F3} | origin:{origin} | end:{origin + Vector3.down * distance}"
            //);
        }
        else
        {
            IsGrounded = false;
            GroundNormal = Vector3.up;
            SlopeAngle = 0f;

            //Debug.Log(
            //    $"NO HIT | origin:{origin} | end:{origin + Vector3.down * distance} | checkDist:{distance} | groundLayerValue:{groundLayer.value}"
            //);
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