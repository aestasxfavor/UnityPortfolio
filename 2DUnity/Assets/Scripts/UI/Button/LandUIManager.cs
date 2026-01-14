using UnityEngine.SceneManagement;
using UnityEngine;

public class LandUIManager : MonoBehaviour
{
    [Header("팝업 참조")]
    [SerializeField] private GameObject exitPopup;
    [SerializeField] private GameObject comingSoonPopup;
    [SerializeField] private bool isActive = false;

    public void ShowExitPopup()
    {
        exitPopup.SetActive(true);
    }

    public void HideExitPopup()
    {
        exitPopup.SetActive(false);
    }

    public void ShowComingSoon()
    {
        isActive = !isActive;
        comingSoonPopup.SetActive(isActive);

    }

    public void HideComingSoon()
    {
        comingSoonPopup.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
