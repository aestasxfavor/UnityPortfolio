using UnityEngine;

public struct MailViewData
{
    public string mailID;
    public string listTitle;
    public string title;
    public string body;
    public bool unlocked;
    public bool isRead;

    public MailViewData(string _id, string _listTitle, string _title, string _body, bool _unlocked, bool _isRead)
    {
        mailID = _id;
        listTitle = _listTitle;
        title = _title;
        body = _body;
        unlocked = _unlocked;
        isRead = _isRead;
    }
}


public class MailBoxService : MonoBehaviour
{
    [SerializeField] private MailDefinition firstMailDefinition; // M001
   
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

    public void SelectMail(string mailID)
    {
        if(!IsInitialized) return;

        if (!mailboxSaveData.firstReturnMailUnlocked) return;

        if(!mailboxSaveData.firstReturnMailRead)
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

        return new MailViewData("M001", firstMailDefinition.ListTitle, firstMailDefinition.Title, firstMailDefinition.Body, unlocked, isRead);


    }
}
