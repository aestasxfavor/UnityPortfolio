using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager Instance;

    private bool isRunning = false;

    private readonly List<UIClosable> oceanClosable = new();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void OnOxygenDepleted()
    {

        ChangeScene(SceneType.Land);
    }

    public void Register(UIClosable closable)
    {
        if (closable == null) return;

        if (!oceanClosable.Contains(closable))
        {
            oceanClosable.Add(closable);
        }
    }

    public void UnRegister(UIClosable closable)
    {
        if (closable == null) return;
        oceanClosable.Remove(closable);
    }

    void CloseAllOceanUI()
    {
        for(int i = oceanClosable.Count - 1; i >= 0; i--)
        {
            oceanClosable[i]?.Close();
        }

        //foreach (var ui in FindObjectsOfType<MonoBehaviour>(true))
        //{
        //    if(ui is UIClosable closable)
        //    {
        //       // Debug.Log($"[SceneFlow] Close UI: {ui.name}");
        //        closable.Close();
        //    }
        //}
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
