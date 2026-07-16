using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneryManager : MonoBehaviour
{
    public static SceneryManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private LoadingText loadingTextWave;
    [SerializeField] private DiveResultUI diveResultUI;

    [Header("Loading Option")]
    [SerializeField] private float minimumLoadingTime = 1.2f;
    [SerializeField] private int minimumLoadingWaveCycles = 4;

    private Coroutine sceneTransitionRoutine;
    private bool isLoading;

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

        bool isReturningToLand = SceneManager.GetActiveScene().name == "Ocean" && sceneName == "Land";

        DisableLandButtonsIfNeeded();
        HideOceanUIForTransitionIfNeeded();
        ShowLoadingScreen();

        if(isReturningToLand)
        {
            ShowDiveResult();
        }
        else
        {
            HideDiveResult();
        }

            float elapsedTime = 0f;

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        loadOperation.allowSceneActivation = false;

        while (loadOperation.progress < 0.9f ||
               elapsedTime < minimumLoadingTime ||
               !HasPlayedMinimumLoadingWave())
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        loadOperation.allowSceneActivation = true;

        while (!loadOperation.isDone)
        {
            yield return null;
        }

        HideLoadingScreen();

        isLoading = false;
        sceneTransitionRoutine = null;
    }

    private void ShowDiveResult()
    {
        if (diveResultUI == null) return;

        int caughtCount = FishInventoryService.Instance?.CurrentDiveCaughtCount ?? 0;
        int newDiscoveryCount = FishInventoryService.Instance?.CurrentDiveNewDiscoveryCount ?? 0;

        diveResultUI.ShowResult(caughtCount, newDiscoveryCount);
    }

    private void HideDiveResult()
    {
        if (diveResultUI == null) return;

        diveResultUI.HideResult();
    }

    private bool HasPlayedMinimumLoadingWave()
    {
        if (loadingTextWave == null)
            return true;

        return loadingTextWave.PlayedCycleCount >= minimumLoadingWaveCycles;
    }

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

    private void HideOceanUIForTransitionIfNeeded()
    {
        OceanManager oceanManager = FindFirstObjectByType<OceanManager>();

        if (oceanManager != null)
        {
            oceanManager.HideOceanUIForTransition();
        }
    }

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
}