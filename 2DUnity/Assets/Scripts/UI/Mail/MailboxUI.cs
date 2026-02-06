using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class MailboxUI : MonoBehaviour
{
    [Header("Mail Data")]
    [SerializeField] private MailDefinition firstMailDefinition; // M001

    [Header("Root Panel")]
    [SerializeField] private GameObject panelRoot;

    [Header("List Item")]
    [SerializeField] private Button firstMailButton;
    [SerializeField] private TMP_Text firstMailTitleText;
    [SerializeField] private GameObject newBadge; // NEW 표시 (선택)

    [Header("Mail Viewer (Right)")]
    [SerializeField] private TMP_Text viewerTitleText;
    [SerializeField] private TMP_Text viewerBodyText;

    [Header("Close Button")]
    [SerializeField] private Button closeButton;

    private MailBoxService mailboxService;

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

    public void Init(MailBoxService service)
    {
        mailboxService = service;
    }

    public void Open()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (mailboxService == null || firstMailDefinition == null) return;

        if (mailboxService == null) return;

        bool unlocked = mailboxService.IsMailUnlocked(null);
        bool isRead = mailboxService.IsMailRead(null);

        if (firstMailButton != null)
        {
            firstMailButton.interactable = unlocked;
        }

        if(firstMailTitleText != null)
        {
            firstMailTitleText.text = unlocked ? firstMailDefinition.ListTitle : "새 편지가 없습니다";
        }

        if(newBadge !=null)
        {
            newBadge.SetActive(unlocked && !isRead);
        }

        // 2) NEW 배지 표시 (언락 && 안읽음)
        // mailService.sc 로 이관 예정 (IsMailNew 함수로 들어갈 예정)
        //if (newBadge != null)
        //    newBadge.SetActive(unlocked && !mailSave.firstReturnMailRead);

        // 3) 우측 뷰어 초기화
        if (!unlocked)
        {
            if (viewerTitleText != null) viewerTitleText.text = "";
            if (viewerBodyText != null) viewerBodyText.text = "";
        }
    
    }

    private void OnClickFirstMail()
    {
        if(mailboxService == null) return;

        if (!mailboxService.IsMailUnlocked(null)) return;

    


        ShowFirstMailInViewer();

        if (!mailboxService.IsMailRead(null))
        {
            mailboxService.MarkAsRead(null);
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
