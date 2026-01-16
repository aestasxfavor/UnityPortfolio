using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoopTester : MonoBehaviour
{
    [Header("Loop Settings")]
    [SerializeField] private SceneType oceanScene = SceneType.Ocean;
    [SerializeField] private SceneType landScene = SceneType.Land;

    [SerializeField, Range(1, 30)] private int loopCount = 10;
    [SerializeField, Range(0.2f, 5f)] private float interval = 3f;

    [Header("Hotkey")]
    [SerializeField] private KeyCode toggleKey = KeyCode.F1;

    private Coroutine loopCoroutine;
    private bool isRunning;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (!isRunning)
                StartLoop();
            else
                StopLoop();
        }
    }

    private void StartLoop()
    {
        if (loopCoroutine != null) return;

        isRunning = true;
        loopCoroutine = StartCoroutine(LoopRoutine());
        Debug.Log($"[SceneLoopTester] START (loopCount={loopCount}, interval={interval})");
    }

    private void StopLoop()
    {
        if (loopCoroutine != null)
        {
            StopCoroutine(loopCoroutine);
            loopCoroutine = null;
        }

        isRunning = false;
        Debug.Log("[SceneLoopTester] STOP");
    }

    private IEnumerator LoopRoutine()
    {
        for (int i = 0; i < loopCount; i++)
        {
            // 1) Ocean -> Land
            if (SceneFlowManager.Instance != null)
                SceneFlowManager.Instance.ChangeScene(landScene);

            yield return new WaitForSeconds(interval);

            // 2) Land -> Ocean
            if (SceneFlowManager.Instance != null)
                SceneFlowManager.Instance.ChangeScene(oceanScene);

            yield return new WaitForSeconds(interval);

            Debug.Log($"[SceneLoopTester] Loop {i + 1}/{loopCount} complete");
            Debug.Log($"sceneCount = {SceneManager.sceneCount}, active = {SceneManager.GetActiveScene().name}");

        }

        loopCoroutine = null;
        isRunning = false;
        Debug.Log("[SceneLoopTester] DONE");
    }
}
