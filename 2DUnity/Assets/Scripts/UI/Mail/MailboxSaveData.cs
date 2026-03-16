using System;
using UnityEngine;
/// <summary>
/// 편지 받음/읽음 상태 저장
/// </summary>
[Serializable]
public class MailboxSaveData
{
    public bool firstReturnMailUnlocked;
    public bool firstReturnMailRead;
}
