using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingUI : MonoBehaviour
{
    [Header("Slider")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Value Text")]
    [SerializeField] private TMP_Text masterValueText;
    [SerializeField] private TMP_Text bgmValueText;
    [SerializeField] private TMP_Text sfxValueText;

    [Header("Buttons")]
    [SerializeField] private Button defaultButton;
    [SerializeField] private Button confirmButton;

    [SerializeField] private GameObject panelRoot;
    private void Awake()
    {
        if (masterSlider != null)
        {
            masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        }

        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
        }

        if (defaultButton != null)
        {
            defaultButton.onClick.AddListener(OnClickDefault);
        }

        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(OnClickConfirm);
        }


    }

    private void OnEnable()
    {
        RefreshUI();
    }

    private void OnDestroy()
    {
        if (masterSlider != null)
        {
            masterSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
        }

        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.RemoveListener(OnBgmVolumeChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveListener(OnSfxVolumeChanged);
        }

        if (defaultButton != null)
        {
            defaultButton.onClick.RemoveListener(OnClickDefault);
        }

        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveListener(OnClickConfirm);
        }


    }

    public void Open()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(true);
        }
        RefreshUI();
    }
    private void RefreshUI()
    {
        if (SoundManager.Instance == null) return;

        if (masterSlider != null)
        {
            masterSlider.SetValueWithoutNotify(SoundManager.Instance.MasterVolume);
        }

        if (bgmSlider != null)
        {
            bgmSlider.SetValueWithoutNotify(SoundManager.Instance.BgmVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.SetValueWithoutNotify(SoundManager.Instance.SfxVolume);
        }

        UpdateValueTexts();
    }

    private void OnMasterVolumeChanged(float value)
    {
        if (SoundManager.Instance == null) return;
        SoundManager.Instance.SetMasterVolume(value);
        UpdateValueText(masterValueText, value);
    }

    private void OnBgmVolumeChanged(float value)
    {
        if (SoundManager.Instance == null) return;
        SoundManager.Instance.SetBgmVolume(value);
        UpdateValueText(bgmValueText, value);
    }

    private void OnSfxVolumeChanged(float value)
    {
        if (SoundManager.Instance == null) return;
        SoundManager.Instance.SetSfxVolume(value);
        UpdateValueText(sfxValueText, value);
    }

    private void OnClickDefault()
    {
        if (SoundManager.Instance == null) return;

        SoundManager.Instance.ResetToDefaultVolumes();
        RefreshUI();
    }

    private void OnClickConfirm()
    {
        if (SoundManager.Instance == null) return;

        SoundManager.Instance.SaveVolumeSettings();
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
    }

    private void UpdateValueTexts()
    {
        if (masterValueText != null)
        {
            UpdateValueText(masterValueText, masterSlider.value);
        }

        if (bgmValueText != null)
        {
            UpdateValueText(bgmValueText, bgmSlider.value);
        }

        if (sfxValueText != null)
        {
            UpdateValueText(sfxValueText, sfxSlider.value);
        }
    }

    private void UpdateValueText(TMP_Text targetText, float value)
    {
        if (targetText == null) return;
        targetText.text = Mathf.RoundToInt(value * 100f).ToString();
    }



}