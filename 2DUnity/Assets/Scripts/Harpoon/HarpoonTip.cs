using UnityEditor.Build.Content;
using UnityEngine;

public class HarpoonTip : MonoBehaviour
{
   [SerializeField] private HarpoonTipConfigSO tipConfig;

    private Vector3 startPos;
    private Vector3 direction;
    private HarpoonPool pool;

    // HarpoonPool에서 연결할 때 호출됨
    public void SetPool(HarpoonPool poolRef)
    {
        pool = poolRef;
    }

    // 작살 발사 시 초기화
    public void Fire(Vector3 dir, HarpoonPool poolRef)
    {
        startPos = transform.position;
        direction = dir.normalized;
        pool = poolRef;
    }

    void Update()
    {
        transform.position += direction * tipConfig.speed * Time.deltaTime;

        if (Vector3.Distance(startPos, transform.position) >= tipConfig.maxDistance)
        {
            pool?.ReturnHarpoon(gameObject);
        }
    }

    void OnEnable()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
    }

    /// <summary>
    /// 2차 리팩토링할 때 할 것
    /// Tag가 아닌 LayCast, OverlapCircle 사용하기
    /// </summary>
    /// <param name="collision"></param>
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeInHierarchy || pool == null) return;

        if (!collision.CompareTag("Fish"))
        {
            pool?.ReturnHarpoon(gameObject);
            return;
        }

        Fish fish = collision.GetComponent<Fish>();
        if (fish == null || fish.isCaught) return;

        fish.isCaught = true;

        //Debug.Log($"[HarpoonTip] {fish.fishType} 잡음");

        //Debug.Log($"[HarpoonTip] 잡은 fishType = {fish.fishType}");
        // 단일 진입점
        FishInventoryService.Instance.AddFish(fish.fishType);

        fish.OnHitByHarpoon();

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Invoke(nameof(ReturnToPool), 0.1f);
    }


    private void ReturnToPool()
    {
        pool?.ReturnHarpoon(gameObject);
    }
}
