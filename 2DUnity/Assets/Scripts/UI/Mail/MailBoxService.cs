using UnityEngine;

/// <summary>
/// Todo: 현재 MailboxService가 Singleton SaveManager에 의존중
///  SaveManager.Instance.RequestSave();
/// 상점까지 싹다 구현 다하고 디테일 작업할 때 의존을 유지할 지 냅둘지 고민하기로
/// </summary>


public class MailboxService : MonoBehaviour
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
