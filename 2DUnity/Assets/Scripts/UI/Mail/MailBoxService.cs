using UnityEngine;

public class MailBoxService : MonoBehaviour
{
    private MailboxSaveData mailboxSaveData;
    public bool IsInitialized {  get; private set; }

    public void Init(MailboxSaveData saveData)
    {
        mailboxSaveData = saveData ?? new MailboxSaveData();
        IsInitialized = true;
    }

    public bool IsMailUnlocked(string mailID)
    {
        // Todo: tq
        if (!IsInitialized) return false;
        return mailboxSaveData.firstReturnMailUnlocked;
    }

    public bool IsMailRead(string mailID)
    {
        if (!IsInitialized) return false;
        return mailboxSaveData.firstReturnMailRead;
    }

    public void Unlock()
    {
        if(!IsInitialized) return;
        if (mailboxSaveData.firstReturnMailUnlocked) return;

        mailboxSaveData.firstReturnMailUnlocked = true;
        SaveManager.Instance.RequestSave();
    }

    public void MarkAsRead(string mailID)
    {
        if (!IsInitialized) return;
        if (mailboxSaveData.firstReturnMailRead) return;

        mailboxSaveData.firstReturnMailRead = true;
        SaveManager.Instance.RequestSave();
    }
}
