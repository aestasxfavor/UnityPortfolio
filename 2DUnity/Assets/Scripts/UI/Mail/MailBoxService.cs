using UnityEngine;

public class MailBoxService : MonoBehaviour
{
    private MailboxSaveData mailboxSaveData;

    public void Init(MailboxSaveData saveData)
    {
        mailboxSaveData = saveData;
    }

    public bool IsMailUnlocked(string mailID)
    {
        return false;
    }

    public void MarkAsRead(string mailID)
    {

    }
}
