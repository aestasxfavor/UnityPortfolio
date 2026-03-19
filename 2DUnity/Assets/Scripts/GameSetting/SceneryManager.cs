using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneryManager : MonoBehaviour
{
    public static SceneryManager Instance;
    [SerializeField] GameObject screen;
    [SerializeField] Slider progress;

    private Coroutine transitionRoutine;
    private bool isLoading;

    private void Awake()
    {
        // 싱글톤 처리
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 시작 시에는 무조건 비활성화
        if (screen != null)
            screen.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        if (isLoading) return;

        if (transitionRoutine != null)
        {
            StopCoroutine(transitionRoutine);

        }

        transitionRoutine = StartCoroutine(TransitionScene(sceneName));
    }

    // SceneryManager.cs 내부

    public IEnumerator TransitionScene(string sceneName)
    {
        isLoading = true;

        var landUI = FindFirstObjectByType<LandUIManager>();
        if (landUI != null)
            landUI.SetButtonUIActive(false);


        if (progress != null) progress.value = 0f;

        if (screen != null) screen.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                progress.value = Mathf.Lerp(progress.value, 1.0f, Time.deltaTime);

                if (progress.value >= 0.99f)
                {
                    asyncOperation.allowSceneActivation = true;

                    yield return null;

                    if (screen != null) screen.SetActive(false);

                    if (sceneName == "Land" && landUI != null)
                    {
                        landUI.SetButtonUIActive(true);
                    }


                    break;
                }
            }
            else
            {
                progress.value = asyncOperation.progress;
            }

            yield return null;
        }

        isLoading = false;
        transitionRoutine = null;
    }

}
