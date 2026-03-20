using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager Instance;

    private readonly List<UIClosable> oceanClosables = new();

    #region Singleton & Lifecycle (ΩÃ±€≈Ê π◊ ª˝∏Ì¡÷±‚)
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

    #region Ocean UI Registration (πŸ¥Ÿ UI µÓ∑œ)
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
        for (int i = oceanClosables.Count - 1; i >= 0; i--)
        {
            if (oceanClosables[i] == null)
            {
                oceanClosables.RemoveAt(i);

                continue;
            }

            oceanClosables[i].Close();
        }
    }
    #endregion

    #region Scene Flow (æ¿ »Â∏ß)
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
        CloseAllOceanUI();
        yield return null;

        GameManager.Instance?.GoToFadeScene(type);
    }
    #endregion
}