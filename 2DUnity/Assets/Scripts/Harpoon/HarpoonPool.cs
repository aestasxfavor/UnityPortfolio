using System.Collections.Generic;
using UnityEngine;

public class HarpoonPool : MonoBehaviour
{
    [Header("풀 설정")]
    [SerializeField] private HarpoonPoolingConfig poolConfig;
    [SerializeField] private GameObject harpoonTipPrefab;

    // 2차 리팩토링 때 상황보고 Stack이나 List로 바꿀 예정
    private readonly Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        if (harpoonTipPrefab == null)
        {
           // Debug.LogError("[HarpoonPool] harpoonTipPrefab이 비어있음");
            return;
        }

        // 최초 풀 생성
        for (int i = 0; i < poolConfig.poolSize; i++)
        {
            GameObject obj = CreateNewHarpoon();
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

       // Debug.Log($"[HarpoonPool] 풀 초기화 완료 ({poolConfig.poolSize}개)");
    }

    // 새 작살 생성 함수 (반복 코드 정리)
    private GameObject CreateNewHarpoon()
    {
        GameObject obj = Instantiate(harpoonTipPrefab, transform);
        HarpoonTip tip = obj.GetComponent<HarpoonTip>();

        if (tip != null)
            tip.SetPool(this);
        else
            Debug.LogWarning("[HarpoonPool] HarpoonTip 컴포넌트를 찾을 수 없음");

        return obj;
    }

    // 작살 요청
    public GameObject GetHarpoon()
    {
        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
           // Debug.LogWarning("[HarpoonPool] 풀 부족 새 작살 생성");
            obj = CreateNewHarpoon();
        }

        obj.SetActive(true);
        return obj;
    }

    // 작살 반환
    public void ReturnHarpoon(GameObject obj)
    {
        if (obj == null)
        {
           // Debug.LogWarning("[HarpoonPool] 반환 시 null 오브젝트 감지");
            return;
        }

        obj.SetActive(false);

        // 중복 반환 방지
        if (!pool.Contains(obj))
        {
            pool.Enqueue(obj);
        }
        else
        {
            Debug.LogWarning("[HarpoonPool] 이미 풀에 존재하는 작살을 다시 반환하려 함");
        }
    }
}
