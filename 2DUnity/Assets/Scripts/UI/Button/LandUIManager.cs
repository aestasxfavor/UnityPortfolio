using UnityEngine.SceneManagement;
using UnityEngine;

public class LandUIManager : MonoBehaviour
{
    [Header("팝업 참조")]
    [SerializeField] private GameObject exitPopup;

    //[SerializeField] private bool isActive = false;
    private MailboxUI mailboxUI;
    private CodexUI codexUI;

    private void Start()
    {
        mailboxUI = FindAnyObjectByType<MailboxUI>(FindObjectsInactive.Include);
        codexUI = FindAnyObjectByType<CodexUI>(FindObjectsInactive.Include);
    }

    public void ToggleCodex()
    {
        // 리팩토링 때 open, close 함수 각각 만들어서 관리할 예정
        if (codexUI == null) return;
        codexUI.CodexToggle();
    }
    public void ShowExitPopup()
    {
        exitPopup.SetActive(true);
    }

    public void HideExitPopup()
    {
        exitPopup.SetActive(false);
    }


    public void ToggleMailBox()
    {
        if (mailboxUI == null) return;
        mailboxUI.MailToggle();
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
