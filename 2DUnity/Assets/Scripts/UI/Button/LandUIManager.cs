using UnityEngine.SceneManagement;
using UnityEngine;

public class LandUIManager : MonoBehaviour
{
    [Header("팝업 참조")]
    [SerializeField] private GameObject exitPopupPrefab;

    [SerializeField] private GameObject storageUICanvasPrefab;
    [SerializeField] private GameObject buttonCanvasPrefab;
    [SerializeField] private GameObject mailboxCanvasPrefab;
    [SerializeField] private GameObject codexCanvasPrefab;

    // Land 생성물 캐시
    private GameObject storageUICanvasInstance;
    private StorageUI storageUIInstance;
    private GameObject buttonCanvasInstance;
    private GameObject exitPopupInstance;
    private GameObject mailboxCanvasInstance;
    private GameObject codexCanvasInstance;

    private MailboxUI mailboxUI;
    private CodexUI codexUI;
    private CodexService codexService;
    private MailBoxService mailBoxService;
    private void Start()
    {
        codexService = FindAnyObjectByType<CodexService>();
        mailBoxService = FindAnyObjectByType<MailBoxService>();
    }

    public void SetUpLand(FishInventoryData storageData)
    {
        if (storageUIInstance == null && storageUICanvasPrefab != null)
        {
            storageUICanvasInstance = Instantiate(storageUICanvasPrefab);
            storageUIInstance = storageUICanvasInstance.GetComponentInChildren<StorageUI>(true);

        }

        if (storageUIInstance != null)
        {
            storageUIInstance.SetInventoryData(storageData);
        }

        if (buttonCanvasInstance == null && buttonCanvasPrefab != null)
        {
            buttonCanvasInstance = Instantiate(buttonCanvasPrefab);

            var buttons = buttonCanvasInstance.GetComponentsInChildren<UnityEngine.UI.Button>(true);

            foreach (var btn in buttons)
            {
                switch (btn.name)
                {
                    case "End Button":
                        Debug.Log("종료 버튼 연결됨");
                        btn.onClick.AddListener(ShowExitPopup);
                        break;

                    case "Mail Button":
                        btn.onClick.AddListener(ToggleMailBox);
                        break;

                    case "Codex Button":
                        btn.onClick.AddListener(ToggleCodex);
                        break;
                }
            }
        }

        if (buttonCanvasInstance != null)
        {
            var storageButton = buttonCanvasInstance.GetComponentInChildren<StorageButton>(true);
            if (storageButton != null && storageUIInstance != null)
            {
                storageButton.SetTargetStorage(storageUIInstance);
            }
        }

        if (codexService != null && !codexService.IsInitialized)
        {
            codexService.Init(SaveManager.Instance.GetCodexSaveData());
        }

        // tq 이거 진짜 그지같은거 나중에 서비스 매니저나 이벤트 만들어서 따로 관리할 예정
        var inventory = FishInventoryService.Instance;
        if (inventory != null)
        {
            inventory.Init(codexService);
        }

        if(mailBoxService != null && !mailBoxService.IsInitialized)
        {
            mailBoxService.Init(SaveManager.Instance.GetMailboxSaveData());
        }
}

public void SetButtonUIActive(bool active)
    {
        if (buttonCanvasInstance != null)
            buttonCanvasInstance.SetActive(active);
    }

    private void ExitPopup()
    {
        if (exitPopupInstance == null && exitPopupPrefab != null)
        {
            exitPopupInstance = Instantiate(exitPopupPrefab);

            exitPopupInstance.SetActive(false);

            var buttons = exitPopupInstance.GetComponentsInChildren<UnityEngine.UI.Button>(true);

            foreach (var btn in buttons)
            {
                switch (btn.name)
                {
                    case "Yes":
                        btn.onClick.AddListener(ExitGame);
                        break;

                    case "No":
                        btn.onClick.AddListener(HideExitPopup);
                        break;
                }
            }
        }
    }
    public void ToggleCodex()
    {
        SetCodex();
        if (codexCanvasInstance == null) return;

        bool active = codexCanvasInstance.activeSelf;
        codexCanvasInstance.SetActive(!active);
        // 리팩토링 때 open, close 함수 각각 만들어서 관리할 예정
        //if (codexUI == null) return;
        //codexUI.CodexToggle();
    }
    public void ShowExitPopup()
    {
        Debug.Log("팝업 열기 시도");
        ExitPopup();
        exitPopupInstance?.SetActive(true);

    }

    public void HideExitPopup()
    {
        ExitPopup();
        exitPopupInstance?.SetActive(false);

    }

    private void SetMailbox()
    {
        if (mailboxCanvasInstance == null && mailboxCanvasPrefab != null)
        {
            mailboxCanvasInstance = Instantiate(mailboxCanvasPrefab);
            mailboxUI = mailboxCanvasInstance.GetComponentInChildren<MailboxUI>(true);
            mailboxUI.Init(mailBoxService);
            mailboxCanvasInstance.SetActive(false);
            //Debug.Log($"[MAIL] instance={mailboxCanvasInstance.name}, active={mailboxCanvasInstance.activeSelf}");
            //Debug.Log($"[MAIL] panelRoot={(mailboxUI != null ? mailboxUI.transform.name : "UI NULL")}");

        }
    }

    private void SetCodex()
    {
        if (codexCanvasInstance == null && codexCanvasPrefab != null)
        {
            codexCanvasInstance = Instantiate(codexCanvasPrefab);
            codexUI = codexCanvasInstance.GetComponentInChildren<CodexUI>(true);
            codexUI.Init(codexService);
            codexCanvasInstance.SetActive(false);
 
        }
    }


    public void ToggleMailBox()
    {
        Debug.Log("메일 버튼 눌림");
        SetMailbox();
        if (mailboxCanvasInstance == null) return;

        bool active = mailboxCanvasInstance.activeSelf;
        Debug.Log("메일 현재 상태: " + active);
        mailboxCanvasInstance.SetActive(!active);

        if (!active & mailboxUI != null)
        {
            mailboxUI.Open();
        }
    }

    public void ExitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
