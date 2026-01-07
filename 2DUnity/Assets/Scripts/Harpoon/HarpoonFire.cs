using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class HarpoonFire : MonoBehaviour
{
    // Todo: 2차 리팩토링 때 SO로 뺄 예정
    [SerializeField] private HarpoonPool harpoonPool;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float harpoonSpeed = 10f;

    public void FireHarpoon(Vector2 direction)
    {
        GameObject harpoon = harpoonPool.GetHarpoon();
        harpoon.transform.position = firePoint.position;
        harpoon.transform.rotation = Quaternion.identity;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        harpoon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Rigidbody2D rb = harpoon.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * harpoonSpeed;

        // 방향에 따라 스프라이트 반전 (선택)
        SpriteRenderer sr = harpoon.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.flipX = direction.x < 0;

        SoundManager.Instance.PlayHarpoonFireSFX();

        // 작살 방향 넘겨주기
        HarpoonTip tip = harpoon.GetComponent<HarpoonTip>();
        if (tip != null)
            tip.Fire(direction, harpoonPool);
    }
}
