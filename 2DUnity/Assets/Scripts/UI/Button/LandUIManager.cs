using UnityEngine.SceneManagement;
using UnityEngine;

public class LandUIManager : MonoBehaviour
{
    [Header("프리팹 연결")]
    [SerializeField] private GameObject exitPopupPrefab;

    [SerializeField] private GameObject storageUICanvasPrefab;
    [SerializeField] private GameObject buttonCanvasPrefab;
    [SerializeField] private GameObject mailboxCanvasPrefab;
    [SerializeField] private GameObject codexCanvasPrefab;

    // Land 생성물 캐시
    private GameObject storageUICanvasInstance;
    private GameObject buttonCanvasInstance;
    private GameObject exitPopupInstance;
    private GameObject mailboxCanvasInstance;
    private GameObject codexCanvasInstance;

    private StorageUI storageUI;
    private MailboxUI mailboxUI;
    private CodexUI codexUI;
    private CodexService codexService;
    private MailboxService mailBoxService;
    private void Start()
    {
        // Todo: FindAnyObjectByType 이것 역시 파일 전체를 돌면서 검사를 하는거라 고칠 필요가 있긴함 
        // 근데 아직은 못함
        codexService = FindAnyObjectByType<CodexService>();
        mailBoxService = FindAnyObjectByType<MailboxService>();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        if (LandUIOpen())
        {
            CloseAllLandUI();
        }
    }

    private bool LandUIOpen()
    {
        if (mailboxCanvasInstance != null && mailboxCanvasInstance.activeSelf) return true;

        if (codexCanvasInstance != null && codexCanvasInstance.activeSelf) return true;

        if (storageUICanvasInstance != null && storageUICanvasInstance.activeSelf) return true;

        return false;
    }

    public void SetUpLand(FishInventoryData storageData)
    {
        if (storageUI == null && storageUICanvasPrefab != null)
        {
            storageUICanvasInstance = Instantiate(storageUICanvasPrefab);
            storageUI = storageUICanvasInstance.GetComponentInChildren<StorageUI>(true);

            var closeButton = storageUICanvasInstance.GetComponentsInChildren<CloseLandUIButton>(true);
            foreach (var button in closeButton)
            {
                button.Init(this);
            }
        }

        if (buttonCanvasInstance == null && buttonCanvasPrefab != null)
        {
            buttonCanvasInstance = Instantiate(buttonCanvasPrefab);

            var buttons = buttonCanvasInstance.GetComponentsInChildren<UnityEngine.UI.Button>(true);

            // Todo: 디테일 작업할 때 3개 묶어서 처리하는 버튼 스크립트 만들어서 enum으로 처리하던가 해야겠다
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

        var storageButton = buttonCanvasInstance.GetComponentInChildren<StorageButton>(true);
        if (storageButton != null)
        {
            storageButton.SetLandUIManager(this);
        }

        if (codexService != null && !codexService.IsInitialized)
        {
            codexService.Init(SaveManager.Instance.GetCodexSaveData());
        }

        // Todo: 이거 진짜 그지같은거 나중에 서비스 매니저나 이벤트 만들어서 따로 관리할 예정
        var inventory = FishInventoryService.Instance;
        if (inventory != null)
        {
            inventory.Init(codexService);
        }

        if (mailBoxService != null && !mailBoxService.IsInitialized)
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
                // Todo: 얘도 마찬가지로 디테일 할때 처리하던가 해야겠음
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

    #region Land UI Open API (리팩토링 4단계)
    public void CloseAllLandUI()
    {
        if (mailboxCanvasInstance != null)
        {
            mailboxCanvasInstance.SetActive(false);
        }

        if (codexCanvasInstance != null)
        {
            codexCanvasInstance.SetActive(false);
        }

        if (storageUICanvasInstance != null)
        {
            storageUICanvasInstance.SetActive(false);
        }

        // Todo: 상점은 나중에 만들기 
    }

    public void OpenMailbox()
    {
        SetMailbox();
        CloseAllLandUI();

        if (mailboxCanvasInstance == null) return;
        mailboxCanvasInstance.SetActive(true);
        mailboxUI.Open();
    }

    public void OpenCodex()
    {
        SetCodex();
        CloseAllLandUI();

        if (codexCanvasInstance == null) return;
        codexCanvasInstance.SetActive(true);
        codexUI.Open();
    }

    public void OpenStorage()
    {
        //Debug.Log($"[OpenStorage] canvas = {storageUICanvasInstance}");
        CloseAllLandUI();
        if (storageUICanvasInstance == null) return;

        storageUICanvasInstance.SetActive(true);
        storageUI.Open();
    }
    #endregion

    public void ToggleCodex()
    {
        OpenCodex();
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

            var closeButton = mailboxCanvasInstance.GetComponentsInChildren<CloseLandUIButton>(true);
            foreach (var button in closeButton)
            {
                button.Init(this);
            }

            mailboxCanvasInstance.SetActive(false);
        }
    }

    private void SetCodex()
    {
        if (codexCanvasInstance == null && codexCanvasPrefab != null)
        {
            codexCanvasInstance = Instantiate(codexCanvasPrefab);
            codexUI = codexCanvasInstance.GetComponentInChildren<CodexUI>(true);
            codexUI.Init(codexService);

            var closeButton = codexCanvasInstance.GetComponentsInChildren<CloseLandUIButton>(true);
            foreach (var button in closeButton)
            {
                button.Init(this);
            }

            codexCanvasInstance.SetActive(false);

        }
    }

    public void ToggleMailBox()
    {
        OpenMailbox();
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
