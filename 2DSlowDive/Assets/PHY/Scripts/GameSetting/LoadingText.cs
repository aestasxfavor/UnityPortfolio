using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private string loadingText = "Loading...";
    [SerializeField] private TextMeshProUGUI letterPrefab;
    [SerializeField] private RectTransform letterRoot;

    [Header("Layout")]
    [SerializeField] private float letterWidth = 44f;
    [SerializeField] private float letterSpacing = 4f;

    [Header("Wave")]
    [SerializeField] private float moveHeight = 12f;
    [SerializeField] private float moveTime = 0.35f;
    [SerializeField] private float delayBetweenLetters = 0.12f;
    [SerializeField] private float loopDelay = 0.45f;

    public int PlayedCycleCount { get; private set; }

    private readonly List<RectTransform> letters = new();
    private readonly List<Vector2> basePositions = new();

    private void Awake()
    {
        CreateLetters();
    }

    private void OnEnable()
    {
        PlayedCycleCount = 0;
        ResetLetters();
        StartCoroutine(WaveRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ResetLetters();
    }

    private void CreateLetters()
    {
        for (int i = letterRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(letterRoot.GetChild(i).gameObject);
        }

        letters.Clear();
        basePositions.Clear();

        int count = loadingText.Length;
        float totalWidth = (count - 1) * (letterWidth + letterSpacing);
        float startX = -totalWidth * 0.5f;

        for (int i = 0; i < count; i++)
        {
            TextMeshProUGUI letter = Instantiate(letterPrefab, letterRoot);
            letter.gameObject.SetActive(true);
            letter.text = loadingText[i].ToString();
            letter.alignment = TextAlignmentOptions.Center;
            letter.raycastTarget = false;

            RectTransform rect = letter.rectTransform;

            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(letterWidth, 70f);

            Vector2 position = new Vector2(startX + i * (letterWidth + letterSpacing), 0f);
            rect.anchoredPosition = position;

            letters.Add(rect);
            basePositions.Add(position);
        }
    }

    private IEnumerator WaveRoutine()
    {
        while (true)
        {
            for (int i = 0; i < letters.Count; i++)
            {
                StartCoroutine(BounceLetter(i));
                yield return new WaitForSecondsRealtime(delayBetweenLetters);
            }

            yield return new WaitForSecondsRealtime(loopDelay);
            PlayedCycleCount++;
        }
    }

    private IEnumerator BounceLetter(int index)
    {
        RectTransform letter = letters[index];

        Vector2 start = basePositions[index];
        Vector2 top = start + Vector2.up * moveHeight;

        float time = 0f;

        while (time < moveTime)
        {
            time += Time.unscaledDeltaTime;
            float t = time / moveTime;
            letter.anchoredPosition = Vector2.Lerp(start, top, Smooth(t));
            yield return null;
        }

        time = 0f;

        while (time < moveTime)
        {
            time += Time.unscaledDeltaTime;
            float t = time / moveTime;
            letter.anchoredPosition = Vector2.Lerp(top, start, Smooth(t));
            yield return null;
        }

        letter.anchoredPosition = start;
    }

    private void ResetLetters()
    {
        for (int i = 0; i < letters.Count; i++)
        {
            if (letters[i] != null && i < basePositions.Count)
            {
                letters[i].anchoredPosition = basePositions[i];
            }
        }
    }

    private float Smooth(float t)
    {
        return t * t * (3f - 2f * t);
    }
}
