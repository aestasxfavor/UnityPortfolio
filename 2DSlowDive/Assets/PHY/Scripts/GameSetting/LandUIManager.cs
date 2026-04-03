using UnityEngine;
using UnityEngine.InputSystem;
public class LandUIManager : MonoBehaviour
{
    #region Prefab References
    [Header("프리팹 연결")]
    [SerializeField] private GameObject exitPopupPrefab;
    [SerializeField] private GameObject storageUICanvasPrefab;
    [SerializeField] private GameObject buttonCanvasPrefab;
    [SerializeField] private GameObject mailboxCanvasPrefab;
    [SerializeField] private GameObject codexCanvasPrefab;
    [SerializeField] private GameObject shopCanvasPrefab;
    [SerializeField] private GameObject soundSettingPrefab;
    #endregion

    #region Cached Instances
    [Header("캐시 생성물")]
    private GameObject storageUICanvasInstance;
    private GameObject buttonCanvasInstance;
    private GameObject exitPopupInstance;
    private GameObject mailboxCanvasInstance;
    private GameObject codexCanvasInstance;
    private GameObject shopCanvasInstance;
    private GameObject soundSettingInstance;
    private GameObject mailButtonBadge;
    private GameObject codexButtonBadge;
    #endregion

    #region Cached Components
    [Header("생성물 컴포넌트")]
    private StorageUI storageUI;
    private MailboxUI mailboxUI;
    private CodexUI codexUI;
    private ShopUI shopUI;
    private SoundSettingUI soundSettingUI;

    private CodexService codexService;
    private MailboxService mailboxService;
    private PopupManager popupManager;
    #endregion

    private PlayerCtrls ctrls;

    #region Unity Lifecycle
    private void Awake()
    {
        ctrls = new PlayerCtrls();
    }

    private void OnEnable()
    {
        ctrls.Enable();
        ctrls.Player.Cancel.performed += OnCancelPerformed;
    }

    private void OnDisable()
    {
        ctrls.Disable();
        ctrls.Player.Cancel.performed -= OnCancelPerformed;
    }
    private void Start()
    {
        codexService = FindAnyObjectByType<CodexService>();
        mailboxService = FindAnyObjectByType<MailboxService>();
        popupManager = FindAnyObjectByType<PopupManager>();
    }

    private void OnCancelPerformed(InputAction.CallbackContext _)
    {
        if (IsAnyLandUIOpen())
        {
            CloseAllLandUI();
        }
    }
    #endregion

    #region Land Setup (육지씬 세팅)
    public void SetUpLand()
    {
        CreateStorageUIIfNeeded();
        CreateMainButtonCanvasIfNeeded();
        ConnectStorageButton();
        InitializeLandServices();
        RefreshMailboxBadge();
        RefreshCodexBadge();
    }

    private void CreateStorageUIIfNeeded()
    {
        if (storageUI != null || storageUICanvasPrefab == null)
            return;

        storageUICanvasInstance = Instantiate(storageUICanvasPrefab);
        storageUI = storageUICanvasInstance.GetComponentInChildren<StorageUI>(true);

        var closeButtons = storageUICanvasInstance.GetComponentsInChildren<CloseLandUIButton>(true);
        foreach (var button in closeButtons)
        {
            button.Init(this);
        }
    }

    private void CreateMainButtonCanvasIfNeeded()
    {
        if (buttonCanvasInstance != null || buttonCanvasPrefab == null)
            return;

        buttonCanvasInstance = Instantiate(buttonCanvasPrefab);

        var buttons = buttonCanvasInstance.GetComponentsInChildren<UnityEngine.UI.Button>(true);
        foreach (var button in buttons)
        {
            ConnectMainButton(button);
        }
    }

    private void ConnectMainButton(UnityEngine.UI.Button button)
    {
        switch (button.name)
        {
            case "End Button":
                button.onClick.AddListener(ShowExitPopup);
                break;

            case "Mail Button":
                button.onClick.AddListener(OnMailboxButtonClicked);
                mailButtonBadge = button.transform.Find("newBadge")?.gameObject;
                break;

            case "Codex Button":
                button.onClick.AddListener(OnCodexButtonClicked);
                codexButtonBadge = button.transform.Find("newBadge")?.gameObject;
                break;

            case "Shop Button":
                button.onClick.AddListener(OnShopButtonClicked);
                break;

            case "Setting Button":
                button.onClick.AddListener(OnSoundButtonClicked);
                break;
        }
    }

    private void ConnectStorageButton()
    {
        if (buttonCanvasInstance == null)
            return;

        var storageButton = buttonCanvasInstance.GetComponentInChildren<StorageButton>(true);
        if (storageButton != null)
        {
            storageButton.SetLandUIManager(this);
        }
    }

    private void InitializeLandServices()
    {
        InitializeCodexService();
        InitializeInventoryService();
        InitializeMailboxService();
    }

    private void InitializeCodexService()
    {
        if (codexService != null && !codexService.IsInitialized)
        {
            codexService.Init(SaveManager.Instance.GetCodexSaveData());
        }
    }

    private void InitializeInventoryService()
    {
        var inventoryService = FishInventoryService.Instance;
        if (inventoryService != null)
        {
            inventoryService.Init(codexService);
        }
    }

    private void InitializeMailboxService()
    {
        if (mailboxService != null && !mailboxService.IsInitialized)
        {
            mailboxService.Init(SaveManager.Instance.GetMailboxSaveData());
        }
    }
    #endregion

    #region Common UI Control (공통 UI 제어)
    private bool IsAnyLandUIOpen()
    {
        if (mailboxCanvasInstance != null && mailboxCanvasInstance.activeSelf) return true;
        if (codexCanvasInstance != null && codexCanvasInstance.activeSelf) return true;
        if (storageUICanvasInstance != null && storageUICanvasInstance.activeSelf) return true;
        if (shopCanvasInstance != null && shopCanvasInstance.activeSelf) return true;
        if (soundSettingInstance != null && soundSettingInstance.activeSelf) return true;
        if (exitPopupInstance != null && exitPopupInstance.activeSelf) return true;

        return false;
    }

    public void CloseAllLandUI()
    {
        if (mailboxCanvasInstance != null)
            mailboxCanvasInstance.SetActive(false);

        if (codexCanvasInstance != null)
            codexCanvasInstance.SetActive(false);

        if (storageUICanvasInstance != null)
            storageUICanvasInstance.SetActive(false);

        if (shopCanvasInstance != null)
            shopCanvasInstance.SetActive(false);

        if (soundSettingInstance != null)
            soundSettingInstance.SetActive(false);

        if(exitPopupInstance != null)
            exitPopupInstance.SetActive(false);

        RefreshMailboxBadge();
        RefreshCodexBadge();
    }

    public void SetButtonUIActive(bool active)
    {
        if (buttonCanvasInstance != null)
            buttonCanvasInstance.SetActive(active);
    }
    #endregion

    #region Mailbox (우편함)
    public void OpenMailbox()
    {
        CreateMailboxUIIfNeeded();
        CloseAllLandUI();

        if (mailboxCanvasInstance == null) return;

        mailboxCanvasInstance.SetActive(true);
        mailboxUI.Open();
    }

    private void CreateMailboxUIIfNeeded()
    {
        if (mailboxCanvasInstance == null && mailboxCanvasPrefab != null)
        {
            mailboxCanvasInstance = Instantiate(mailboxCanvasPrefab);
            mailboxUI = mailboxCanvasInstance.GetComponentInChildren<MailboxUI>(true);
            mailboxUI.Init(mailboxService);

            var closeButtons = mailboxCanvasInstance.GetComponentsInChildren<CloseLandUIButton>(true);
            foreach (var button in closeButtons)
            {
                button.Init(this);
            }

            mailboxCanvasInstance.SetActive(false);
        }
    }

    private void RefreshMailboxBadge()
    {
        if (mailButtonBadge == null) return;

        if (mailboxService == null || !mailboxService.IsInitialized)
        {
            mailButtonBadge.SetActive(false);
            return;
        }

        bool hasUnreadMail = mailboxService.HasUnreadMail();
        mailButtonBadge.SetActive(hasUnreadMail);
    }

    public void OnMailboxButtonClicked()
    {
        OpenMailbox();
    }
    #endregion

    #region Codex (도감)
    public void OpenCodex()
    {
        CreateCodexUIIfNeeded();
        CloseAllLandUI();

        if (codexCanvasInstance == null) return;

        codexCanvasInstance.SetActive(true);
        codexUI.Open();
    }

    private void CreateCodexUIIfNeeded()
    {
        if (codexCanvasInstance == null && codexCanvasPrefab != null)
        {
            codexCanvasInstance = Instantiate(codexCanvasPrefab);
            codexUI = codexCanvasInstance.GetComponentInChildren<CodexUI>(true);
            codexUI.Init(codexService);

            var closeButtons = codexCanvasInstance.GetComponentsInChildren<CloseLandUIButton>(true);
            foreach (var button in closeButtons)
            {
                button.Init(this);
            }

            codexCanvasInstance.SetActive(false);
        }
    }

    private void RefreshCodexBadge()
    {
        if (codexButtonBadge == null) return;

        if (codexService == null || !codexService.IsInitialized)
        {
            codexButtonBadge.SetActive(false);
            return;
        }

        bool hasNewCodex = codexService.HasUnViewedNewFish();
        codexButtonBadge.SetActive(hasNewCodex);
    }

    public void OnCodexButtonClicked()
    {
        OpenCodex();
    }
    #endregion

    #region Shop (교환상점)
    public void OpenShop()
    {
        CreateShopUIIfNeeded();
        CloseAllLandUI();

        if (shopCanvasInstance == null) return;

        shopCanvasInstance.SetActive(true);
        shopUI.Open();
    }

    private void CreateShopUIIfNeeded()
    {
        if (shopCanvasInstance == null && shopCanvasPrefab != null)
        {
            shopCanvasInstance = Instantiate(shopCanvasPrefab);
            shopUI = shopCanvasInstance.GetComponentInChildren<ShopUI>(true);
            shopUI.Init(popupManager);

            var closeButtons = shopCanvasInstance.GetComponentsInChildren<CloseLandUIButton>(true);
            foreach (var button in closeButtons)
            {
                button.Init(this);
            }

            shopCanvasInstance.SetActive(false);
        }
    }

    public void OnShopButtonClicked()
    {
        OpenShop();
    }
    #endregion

    #region Storage (보관함)
    public void OpenStorage()
    {
        CloseAllLandUI();

        if (storageUICanvasInstance == null) return;

        storageUICanvasInstance.SetActive(true);
        storageUI.Open();
    }
    #endregion

    #region Sound Setting (소리 설정)
    public void OpenSoundSetting()
    {
        CreateSoundSettingUIIfNeeded();
        CloseAllLandUI();

        if (soundSettingInstance == null) return;

        soundSettingInstance.SetActive(true);
        soundSettingUI.Open();
    }

    private void CreateSoundSettingUIIfNeeded()
    {
        if (soundSettingInstance == null && soundSettingPrefab != null)
        {
            soundSettingInstance = Instantiate(soundSettingPrefab);
            soundSettingUI = soundSettingInstance.GetComponentInChildren<SoundSettingUI>(true);
            soundSettingInstance.SetActive(false);
        }
    }

    public void OnSoundButtonClicked()
    {
        OpenSoundSetting();
    }
    #endregion

    #region Exit Popup (종료창 팝업)
    public void ShowExitPopup()
    {
        CreateExitPopupIfNeeded();
        exitPopupInstance?.SetActive(true);
    }

    public void HideExitPopup()
    {
        CreateExitPopupIfNeeded();
        exitPopupInstance?.SetActive(false);
    }

    private void CreateExitPopupIfNeeded()
    {
        if (exitPopupInstance == null && exitPopupPrefab != null)
        {
            exitPopupInstance = Instantiate(exitPopupPrefab);
            exitPopupInstance.SetActive(false);

            ConnectExitPopupButtons();
        }
    }

    private void ConnectExitPopupButtons()
    {
        var buttons = exitPopupInstance.GetComponentsInChildren<UnityEngine.UI.Button>(true);

        foreach (var button in buttons)
        {
            switch (button.name)
            {
                case "Yes":
                    button.onClick.AddListener(ExitGame);
                    break;

                case "No":
                    button.onClick.AddListener(HideExitPopup);
                    break;
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    #endregion
}