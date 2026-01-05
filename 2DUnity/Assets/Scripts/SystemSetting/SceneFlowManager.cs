using System.Collections;
using UnityEngine;

public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager Instance;

    private bool isRunning = false;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnOxygenDepleted()
    {

        ChangeScene(SceneType.Land);
    }

    void CloseAllOceanUI()
    {
        foreach (var ui in FindObjectsOfType<MonoBehaviour>(true))
        {
            if(ui is UIClosable closable)
            {
                Debug.Log($"[SceneFlow] Close UI: {ui.name}");
                closable.Close();
            }
        }
    }

   public void ChangeScene(SceneType type)
    {
        if (isRunning) return;
        isRunning = true;

        StartCoroutine(ChangeSceneRoutine(type));
    }

    private IEnumerator ChangeSceneRoutine(SceneType type)
    {
        // ¹Ù´Ù¾À UI Á¾·á
        CloseAllOceanUI();
        yield return null;

        GameManager.Instance.GoToFadeScene(type);

        isRunning = false;
    }
}
