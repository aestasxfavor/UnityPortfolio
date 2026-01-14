using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleTextUI : MonoBehaviour
{
    [SerializeField] private Image targetImage; // Touch to Start 이미지

    // Todo: 상점, 도감, 유리병편지까지 전부 다 끝내고 마무리 작업할때 SO로 빼든가 할 예정
    [SerializeField] private float blinkSpeed = 1.0f;

    void Start()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * blinkSpeed;
                float alpha = Mathf.PingPong(t, 1f); // 0~1 반복
                Color color = targetImage.color;
                color.a = alpha;
                targetImage.color = color;
                yield return null;
            }
        }
    }
}
