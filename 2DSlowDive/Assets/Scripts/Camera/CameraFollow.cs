using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Quaternion fixedRotation;

    void Start()
    {
        // 현재 카메라 회전값을 기준으로 고정
        fixedRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // 매 프레임마다 회전을 덮어써서 부모 회전 상속을 무시
        transform.rotation = fixedRotation;
    }
}
