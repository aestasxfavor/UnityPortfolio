using UnityEngine;

public class SeaSpacle : MonoBehaviour
{
    [Header("Alpha")]
    [SerializeField] private float minAlpha = 0f;
    [SerializeField] private float maxAlpha = 0.16f;
    [SerializeField] private float flickerSpeed = 0.6f;

    [Header("Scale")]
    [SerializeField] private float minScale = 0.9f;
    [SerializeField] private float maxScale = 1.05f;

    [Header("Drift")]
    [SerializeField] private Vector2 driftAmount = new Vector2(0.03f, 0.005f);
    [SerializeField] private float driftSpeed = 0.2f;

    private SpriteRenderer[] renderers;
    private Vector3[] startPositions;
    private Vector3[] startScales;
    private Color[] startColors;
    private float[] seeds;

    private void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();

        startPositions = new Vector3[renderers.Length];
        startScales = new Vector3[renderers.Length];
        startColors = new Color[renderers.Length];
        seeds = new float[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            Transform child = renderers[i].transform;

            startPositions[i] = child.localPosition;
            startScales[i] = child.localScale;
            startColors[i] = renderers[i].color;
            seeds[i] = Random.Range(0f, 100f);
        }
    }

    private void Update()
    {
        float time = Time.unscaledTime;

        for (int i = 0; i < renderers.Length; i++)
        {
            Transform child = renderers[i].transform;
            float t = time + seeds[i];

            float alpha01 = Mathf.Sin(t * flickerSpeed) * 0.5f + 0.5f;
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, alpha01);

            Color color = startColors[i];
            color.a = alpha;
            renderers[i].color = color;

            float scale01 = Mathf.Sin(t * flickerSpeed * 0.8f) * 0.5f + 0.5f;
            float scale = Mathf.Lerp(minScale, maxScale, scale01);
            child.localScale = startScales[i] * scale;

            float driftX = Mathf.Sin(t * driftSpeed) * driftAmount.x;
            float driftY = Mathf.Cos(t * driftSpeed * 0.7f) * driftAmount.y;
            child.localPosition = startPositions[i] + new Vector3(driftX, driftY, 0f);
        }
    }
}
