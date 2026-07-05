using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager Instance;

    private readonly List<UIClosable> oceanClosables = new();

    #region Singleton & Lifecycle (½Ì±ÛÅæ ¹× »ý¸íÁÖ±â)
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    #endregion

    #region Ocean UI Registration (¹Ù´Ù UI µî·Ï)
    public void Register(UIClosable closable)
    {
        if (closable == null) return;

        if (!oceanClosables.Contains(closable))
        {
            oceanClosables.Add(closable);
        }
    }

    public void Unregister(UIClosable closable)
    {
        if (closable == null) return;

        oceanClosables.Remove(closable);
    }

    private void CloseAllOceanUI()
    {
        List<UIClosable> targets = new(oceanClosables);
        oceanClosables.Clear();

        foreach (UIClosable closable in targets)
        {
            if (closable == null)
                continue;

            closable.Close();
        }
    }
    #endregion

    #region Scene Flow (¾À Èå¸§)
    public void OnOxygenDepleted()
    {
        ChangeScene(SceneType.Land);
    }

    public void ChangeScene(SceneType type)
    {
        StartCoroutine(ChangeSceneRoutine(type));
    }

    private IEnumerator ChangeSceneRoutine(SceneType type)
    {
        HideOceanUIForTransitionIfNeeded();

        yield return null;

        GameManager.Instance?.GoToFadeScene(type);
    }

    private void HideOceanUIForTransitionIfNeeded()
    {
        OceanManager oceanManager = FindFirstObjectByType<OceanManager>();

        if (oceanManager != null)
        {
            oceanManager.HideOceanUIForTransition();
        }
    }
    #endregion
}