using System.Collections;
using UnityEngine;

public enum FishType
{
    Blue,   // 0, 파도꼬리
    Orange, // 1, 보거
    Red,    // 2, 빨강눈치
    Green,  // 3, 초록멍치
    Shark,  // 4, 아기상어
    Grey,   // 5, 출근고등어
    JellyFish,  // 6, 멍파리
    Octopus,    // 7, 팔동문어
    Shrimp,     // 8, 등짝새우
    SwordFish,  // 9, 질주청새치
    Squid,     // 10, 수줍오징어
        // 심해어 2종은 2차 리팩하고 상점이랑 도감 만들때 하지뭐
}

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Fish : MonoBehaviour
{
    [Header("물고기 기본 설정")]
    public FishType fishType;

    [Tooltip("JSON에 등록된 이름 (FishType과 동일)")]
    [SerializeField] private string fishName;

    [SerializeField] protected FishConfigSO fishConfig;


    [HideInInspector] public bool isCaught = false;

    // 내부 컴포넌트
    protected Rigidbody2D rigid;
    protected SpriteRenderer spriteRenderer;

    // 이동 관련
    protected bool isMovingRight = true;
    protected Vector3 startPos;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;

        // fishName 자동 지정 (enum 이름과 일치시킴)
        if (string.IsNullOrEmpty(fishName))
            fishName = fishType.ToString();

        if (fishConfig == null) return;
    }

    private void OnEnable()
    {
        isCaught = false;
        startPos = transform.position;
        //Debug.Log($"[Fish Spawn] name={gameObject.name} fishType={fishType}");
    }

    private void FixedUpdate()
    {
        if (isCaught) return; // 잡힌 상태면 움직이지 않게

        float moveDir = isMovingRight ? 1f : -1f;
        rigid.linearVelocity = new Vector2(moveDir * fishConfig.moveSpeed, rigid.linearVelocity.y);

        float distanceMoved = transform.position.x - startPos.x;

        if (Mathf.Abs(distanceMoved) >= fishConfig.moveDistance)
        {
            isMovingRight = !isMovingRight;
            startPos = transform.position;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    /// <summary>
    /// 작살에 맞았을 때 호출됨 (HarpoonTip → OceanManager로 처리)
    /// </summary>
    public void OnHitByHarpoon()
    {
        isCaught = true;
        rigid.linearVelocity = Vector2.zero;
        StartCoroutine(Vanish());
        //Debug.Log($"[Fish] {fishName} 잡힘 → 비활성화 예정");
    }

    /// <summary>
    /// 피격 후 비활성화 연출
    /// </summary>
    protected IEnumerator Vanish()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// OceanManager로 전달할 고정 이름 반환
    /// </summary>
    public string GetFishName()
    {
        return fishName;
    }
}
