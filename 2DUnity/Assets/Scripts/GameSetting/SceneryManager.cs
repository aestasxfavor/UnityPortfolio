using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneryManager : MonoBehaviour
{
    public static SceneryManager Instance;

    #region UI References (UI 참조)
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingProgressBar;
    #endregion

    #region State (상태)
    private Coroutine sceneTransitionRoutine;
    private bool isLoading;
    #endregion

    #region Singleton & Lifecycle (싱글톤 및 생명주기)
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        HideLoadingScreen();
    }
    #endregion

    #region Scene Transition (씬 전환)
    public void LoadScene(string sceneName)
    {
        if (isLoading)
            return;

        StopSceneTransitionRoutineIfRunning();
        sceneTransitionRoutine = StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        isLoading = true;

        DisableLandButtonsIfNeeded();
        ResetLoadingProgress();
        ShowLoadingScreen();

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        loadOperation.allowSceneActivation = false;

        while (!loadOperation.isDone)
        {
            UpdateLoadingProgress(loadOperation);

            if (IsSceneReadyToActivate(loadOperation))
            {
                loadOperation.allowSceneActivation = true;
                yield return null;

                HideLoadingScreen();
                break;
            }

            yield return null;
        }

        isLoading = false;
        sceneTransitionRoutine = null;
    }
    #endregion

    #region Scene Transition Helpers (씬 전환 보조)
    private void StopSceneTransitionRoutineIfRunning()
    {
        if (sceneTransitionRoutine == null)
            return;

        StopCoroutine(sceneTransitionRoutine);
        sceneTransitionRoutine = null;
    }

    private void DisableLandButtonsIfNeeded()
    {
        var landUIManager = FindFirstObjectByType<LandUIManager>();
        if (landUIManager != null)
        {
            landUIManager.SetButtonUIActive(false);
        }
    }

    private void ResetLoadingProgress()
    {
        if (loadingProgressBar != null)
        {
            loadingProgressBar.value = 0f;
        }
    }

    private void UpdateLoadingProgress(AsyncOperation loadOperation)
    {
        if (loadingProgressBar == null)
            return;

        if (loadOperation.progress >= 0.9f)
        {
            loadingProgressBar.value = Mathf.Lerp(loadingProgressBar.value, 1.0f, Time.deltaTime);
        }
        else
        {
            loadingProgressBar.value = loadOperation.progress;
        }
    }

    private bool IsSceneReadyToActivate(AsyncOperation loadOperation)
    {
        if (loadOperation.progress < 0.9f)
            return false;

        if (loadingProgressBar == null)
            return true;

        return loadingProgressBar.value >= 0.99f;
    }
    #endregion

    #region Loading Screen Control (로딩 화면 제어)
    private void ShowLoadingScreen()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }
    }

    private void HideLoadingScreen()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }
    #endregion
}