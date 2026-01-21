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

    private void Awake()
    {
        if (panelRoot != null)
            panelRoot.SetActive(false);

        if (firstMailButton != null)
            firstMailButton.onClick.AddListener(OnClickFirstMail);

        if (closeButton != null)
            closeButton.onClick.AddListener(Close);
    }

    private void OnDestroy()
    {
        if (firstMailButton != null)
            firstMailButton.onClick.RemoveListener(OnClickFirstMail);

        if (closeButton != null)
            closeButton.onClick.RemoveListener(Close);
    }

    // -------------------------
    // Public
    // -------------------------

    public void Open()
    {
        if (panelRoot == null) return;

        panelRoot.SetActive(true);
        Refresh();
    }

    public void Close()
    {
        if (panelRoot == null) return;
        panelRoot.SetActive(false);
    }

    public void Toggle()
    {
        Debug.Log($"[MAILBOX] Toggle called / this={gameObject.name} / panelRoot={(panelRoot == null ? "NULL" : panelRoot.name)}");

        if (panelRoot == null) return;

        if (panelRoot.activeInHierarchy) Close();
        else Open();
    }

    // -------------------------
    // UI Update
    // -------------------------

    private void Refresh()
    {
        // 안전장치
        if (SaveManager.Instance == null || firstMailDefinition == null)
            return;

        var mailSave = SaveManager.Instance.GetMailboxSaveData();

        // 1) 언락 여부에 따른 버튼 활성화
        bool unlocked = mailSave.firstReturnMailUnlocked;

        if (firstMailButton != null)
            firstMailButton.interactable = unlocked;

        if (firstMailTitleText != null)
            firstMailTitleText.text = unlocked ? firstMailDefinition.ListTitle : "새 편지가 없습니다";


        // 2) NEW 배지 표시 (언락 && 안읽음)
        if (newBadge != null)
            newBadge.SetActive(unlocked && !mailSave.firstReturnMailRead);

        // 3) 우측 뷰어 초기화
        if (!unlocked)
        {
            if (viewerTitleText != null) viewerTitleText.text = "";
            if (viewerBodyText != null) viewerBodyText.text = "";
        }
    
    }

    private void OnClickFirstMail()
    {
        if (SaveManager.Instance == null || firstMailDefinition == null)
            return;

        var mailSave = SaveManager.Instance.GetMailboxSaveData();
        if (!mailSave.firstReturnMailUnlocked)
            return;

        ShowFirstMailInViewer();

        // 읽음 처리
        if (!mailSave.firstReturnMailRead)
        {
            mailSave.firstReturnMailRead = true;
            SaveManager.Instance.RequestSave();
        }

        // NEW 배지 끄기
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
