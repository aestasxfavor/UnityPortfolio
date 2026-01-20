using UnityEngine;
/// <summary>
/// 편지 내용 데이터
/// </summary>
[CreateAssetMenu(fileName = "MailDefinition", menuName = "Scriptable Objects/MailDefinition")]
public class MailDefinition : ScriptableObject
{
    [Header("ID (Save Key)")]
    [SerializeField] private string mailId = "M001";

    [Header("UI")]
    [SerializeField] private string title;

    [TextArea(6, 20)]
    [SerializeField] private string body;

    public string MailId => mailId;
    public string Title => title;
    public string Body => body;
}
