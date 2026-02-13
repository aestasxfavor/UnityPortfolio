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
