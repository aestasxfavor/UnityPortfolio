using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public enum HarpoonType
{
    Normal,
    Upgrade
}

public class HarpoonFire : MonoBehaviour
{
    [SerializeField] private HarpoonPool harpoonPool;
    [SerializeField] private Transform firePoint;
    [SerializeField] private HarpoonFireConfigSO fireConfig;

    [SerializeField] private HarpoonType currentHarpoonType = HarpoonType.Normal;


    //public void FireHarpoon(Vector2 direction)
    //{
    //    GameObject harpoon = harpoonPool.GetHarpoon();
    //    harpoon.transform.position = firePoint.position;
    //    harpoon.transform.rotation = Quaternion.identity;

    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    harpoon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    //    Rigidbody2D rb = harpoon.GetComponent<Rigidbody2D>();
    //    rb.linearVelocity = direction.normalized * fireConfig.harpoonSpeed;

    //    // 방향에 따라 스프라이트 반전 (선택)
    //    SpriteRenderer sr = harpoon.GetComponent<SpriteRenderer>();
    //    if (sr != null)
    //        sr.flipX = direction.x < 0;

    //    if (SoundManager.Instance != null)
    //        SoundManager.Instance.PlayHarpoonFireSFX();


    //    // 작살 방향 넘겨주기
    //    HarpoonTip tip = harpoon.GetComponent<HarpoonTip>();
    //    if (tip != null)
    //        tip.Fire(direction, harpoonPool);
    //}

    public void SetHarpoonType(HarpoonType type)
    {
        currentHarpoonType = type;
    }

    public void FireHarpoon(Vector2 direction)
    {
        Debug.Log($"Fire 호출 타입: {currentHarpoonType}");

        int shootCount = 1;
        float spreadAngle = 0f;

        if (currentHarpoonType == HarpoonType.Upgrade)
        {
            shootCount = 3;
            spreadAngle = 45f;
        }

        float startAngle = -(spreadAngle * (shootCount - 1) / 2f);

        for (int i = 0; i < shootCount; i++)
        {
            float angleOffset = startAngle + (spreadAngle * i);

            Vector2 newDir =
                Quaternion.Euler(0, 0, angleOffset) * direction.normalized;

            SpawnHarpoon(newDir);
        }

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayHarpoonFireSFX();
    }

    private void SpawnHarpoon(Vector2 dir)
    {
        GameObject harpoon = harpoonPool.GetHarpoon();

        harpoon.transform.position = firePoint.position;
        harpoon.transform.rotation = Quaternion.identity;

        Rigidbody2D rb = harpoon.GetComponent<Rigidbody2D>();

        // 이거 반드시 필요
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        harpoon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        rb.linearVelocity = dir.normalized * fireConfig.harpoonSpeed;

        SpriteRenderer sr = harpoon.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.flipX = dir.x < 0;

        HarpoonTip tip = harpoon.GetComponent<HarpoonTip>();
        if (tip != null)
            tip.Fire(dir, harpoonPool);
    }
}

