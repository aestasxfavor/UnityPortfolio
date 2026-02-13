using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class MailboxUI : MonoBehaviour
{
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

    private MailboxService mailboxService;
    private bool isMailSelected = false;

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

    public void Init(MailboxService service)
    {
        mailboxService = service;
    }

    public void Open()
    {
        if (panelRoot != null)
        {
            Debug.Log("우편함 활성화됨");
            panelRoot.SetActive(true);
        }
        Refresh();
    }

    private void Refresh()
    {
        if (mailboxService == null) return;

        var mail = mailboxService.GetFirstMailViewData();

        firstMailButton.interactable = mail.unlocked;

        firstMailTitleText.text = mail.unlocked
            ? mail.listTitle : "새 편지가 없습니다";

        // Todo: NewBage는 상점까지 싹 다하고 디테일 작업때 할 예정
        if (newBadge != null)
            newBadge.SetActive(mail.unlocked && !mail.isRead);

        if (!mail.unlocked)
        {
            viewerTitleText.text = "";
            viewerBodyText.text = "";
        }
    }

    private void OnClickFirstMail()
    {
        if(mailboxService == null) return;
        if (!mailboxService.IsMailUnlocked(null)) return;

        if(isMailSelected)
        {
            isMailSelected = false;
            HideViewer();
            return;
        }
        isMailSelected = true;

        mailboxService.SelectMail(null);

        ShowFirstMailInViewer();
        Refresh();

    }

    private void ShowFirstMailInViewer()
    {
        if (mailboxService == null) return;

        var mail = mailboxService.GetFirstMailViewData();

        if (viewerTitleText != null)
            viewerTitleText.text = mail.title;

        if (viewerBodyText != null)
            viewerBodyText.text = mail.body;
    }

    private void HideViewer()
    {
        if(viewerTitleText !=null)
        {
            viewerTitleText.text = "";
        }

        if(viewerBodyText != null)
        {
            viewerBodyText.text = "";
        }

    }

}
