using UnityEngine;

public class MailboxService : MonoBehaviour
{
    [SerializeField] private MailDefinition firstMailDefinition;

    private MailboxSaveData mailboxSaveData;
    public bool IsInitialized { get; private set; }

    public void Init(MailboxSaveData saveData)
    {
        mailboxSaveData = saveData ?? new MailboxSaveData();
        IsInitialized = true;
    }

    public bool IsMailUnlocked(string mailID)
    {
        if (!IsInitialized) return false;
        return mailboxSaveData.firstReturnMailUnlocked;
    }

    public bool IsMailRead(string mailID)
    {
        if (!IsInitialized) return false;
        return mailboxSaveData.firstReturnMailRead;
    }

    public bool HasUnreadMail()
    {
        if (!IsInitialized) return false;
        return mailboxSaveData.firstReturnMailUnlocked && !mailboxSaveData.firstReturnMailRead;
    }

    public void Unlock()
    {
        if (!IsInitialized) return;
        if (mailboxSaveData.firstReturnMailUnlocked) return;

        mailboxSaveData.firstReturnMailUnlocked = true;
        SaveManager.Instance.SaveMailbox();
    }

    public void MarkAsRead(string mailID)
    {
        if (!IsInitialized) return;
        if (mailboxSaveData.firstReturnMailRead) return;

        mailboxSaveData.firstReturnMailRead = true;
        SaveManager.Instance.SaveMailbox();
    }

    public void SelectMail(string mailID)
    {
        if (!IsInitialized) return;
        if (!mailboxSaveData.firstReturnMailUnlocked) return;

        if (!mailboxSaveData.firstReturnMailRead)
        {
            mailboxSaveData.firstReturnMailRead = true;
            SaveManager.Instance.RequestSave();

        }
    }

    public MailViewData GetFirstMailViewData()
    {
        if (!IsInitialized || firstMailDefinition == null) return default;

        bool unlocked = mailboxSaveData.firstReturnMailUnlocked;
        bool isRead = mailboxSaveData.firstReturnMailRead;

        return new MailViewData
            (
            "M001",
            firstMailDefinition.ListTitle,
            firstMailDefinition.Title,
            firstMailDefinition.Body,
            unlocked,
            isRead
        );
    }
}