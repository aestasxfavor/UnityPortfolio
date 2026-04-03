using UnityEngine;

public class Harpoon : MonoBehaviour
{
    [SerializeField] private Transform followTarget; // 보통 플레이어
    [SerializeField] private Vector3 offset;         // 손 위치에 맞게 오프셋 조정
    [SerializeField] private bool followRotation = true;

    void Update()
    {
        if (followTarget == null) return;

        // 위치 따라가기
        transform.position = followTarget.position + offset;

        // 회전도 따라가게 (플레이어가 Z축 회전 중일 때)
        if (followRotation)
        {
            transform.rotation = followTarget.rotation;
        }
    }

    public void SetFollowTarget(Transform target, Vector3 customOffset)
    {
        followTarget = target;
        offset = customOffset;
    }
}
