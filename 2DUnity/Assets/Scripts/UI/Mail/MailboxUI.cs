using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class MailboxUI : MonoBehaviour
{
    [Header("Mail Data")]
    [SerializeField] private MailDefinition firstMailDefinition; // M001

    [Header("Root Panel")]
    [SerializeField] private GameObject panelRoot;
    //[SerializeField] private GameObject canvasRoot; // MailCanvas


    [Header("List Item")]
    [SerializeField] private Button firstMailButton;
    [SerializeField] private TMP_Text firstMailTitleText;
    [SerializeField] private GameObject newBadge; // NEW 표시 (선택)

    [Header("Mail Viewer (Right)")]
    [SerializeField] private TMP_Text viewerTitleText;
    [SerializeField] private TMP_Text viewerBodyText;

    [Header("Close Button")]
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        if (firstMailButton != null)
        {
            firstMailButton.onClick.AddListener(OnClickFirstMail);

        }
    }

    private void OnDestroy()
    {
        if (firstMailButton != null)
        {
            firstMailButton.onClick.RemoveListener(OnClickFirstMail);

        }
    }

    // -------------------------
    // Public
    // -------------------------

    public void Open()
    {
        //if (panelRoot != null)
        //    panelRoot.SetActive(true);


        Refresh();
    }

    // -------------------------
    // UI Update
    // -------------------------

    private void Refresh()
    {
        // 안전장치
        if (SaveManager.Instance == null || firstMailDefinition == null)
            return;

        // mailService.sc 로 이관 예정
        var mailSave = SaveManager.Instance.GetMailboxSaveData();

        // mailService.sc 로 이관 예정 (IsMailUnlocked 함수로 들어갈 예정)
        bool unlocked = mailSave.firstReturnMailUnlocked;

        if (firstMailButton != null)
            firstMailButton.interactable = unlocked;

        if (firstMailTitleText != null)
            firstMailTitleText.text = unlocked ? firstMailDefinition.ListTitle : "새 편지가 없습니다";


        // 2) NEW 배지 표시 (언락 && 안읽음)
        // mailService.sc 로 이관 예정 (IsMailNew 함수로 들어갈 예정)
        if (newBadge != null)
            newBadge.SetActive(unlocked && !mailSave.firstReturnMailRead);

        // 3) 우측 뷰어 초기화
        if (!unlocked)
        {
            if (viewerTitleText != null) viewerTitleText.text = "";
            if (viewerBodyText != null) viewerBodyText.text = "";
        }
    
    }

    private void OnClickFirstMail()
    {
        if (SaveManager.Instance == null || firstMailDefinition == null)
            return;
        // Todo: MailBoxService.sc로 이관 예정
        var mailSave = SaveManager.Instance.GetMailboxSaveData();
        
        if (!mailSave.firstReturnMailUnlocked)
            return;

        ShowFirstMailInViewer();

        // 읽음 처리
        if (!mailSave.firstReturnMailRead)
        {
            mailSave.firstReturnMailRead = true;
            SaveManager.Instance.RequestSave();
            // Todo: MailBoxService.sc로 이관 예정(MarkAsRead 함수로 들어갈 예정)
        }

        // NEW 배지 끄기
        // Todo: New 배지 상태 판단 MailBoxService.sc에서 담당 할 예정
        if (newBadge != null)
            newBadge.SetActive(false);
    }

    private void ShowFirstMailInViewer()
    {
        if (viewerTitleText != null)
            viewerTitleText.text = firstMailDefinition.Title;

        if (viewerBodyText != null)
            viewerBodyText.text = firstMailDefinition.Body;
    }

}
