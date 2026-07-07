using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundSettingUI : MonoBehaviour, IPointerDownHandler
{
    [Header("Slider")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Slider Click Area")]
    [SerializeField] private RectTransform masterClickArea;
    [SerializeField] private RectTransform bgmClickArea;
    [SerializeField] private RectTransform sfxClickArea;

    [Header("Value Text")]
    [SerializeField] private TMP_Text masterValueText;
    [SerializeField] private TMP_Text bgmValueText;
    [SerializeField] private TMP_Text sfxValueText;

    [Header("Buttons")]
    [SerializeField] private Button defaultButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button closemButton;

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

        if (closemButton != null)
        {
            closemButton.onClick.AddListener(OnClickClose);
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

        if (closemButton != null)
        {
            closemButton.onClick.RemoveListener(OnClickClose);
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (TrySetSliderByClick(masterSlider, masterClickArea, eventData))
        {
            return;
        }

        if (TrySetSliderByClick(bgmSlider, bgmClickArea, eventData))
        {
            return;
        }

        if (TrySetSliderByClick(sfxSlider, sfxClickArea, eventData))
        {
            return;
        }
    }

    private bool TrySetSliderByClick(Slider slider, RectTransform clickArea, PointerEventData eventData)
    {
        if (slider == null || clickArea == null)
        {
            return false;
        }

        if (!RectTransformUtility.RectangleContainsScreenPoint(
            clickArea,
            eventData.position,
            eventData.pressEventCamera))
        {
            return false;
        }

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            clickArea,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint))
        {
            return false;
        }

        Rect rect = clickArea.rect;
        float normalizedValue = Mathf.InverseLerp(rect.xMin, rect.xMax, localPoint.x);

        slider.normalizedValue = Mathf.Clamp01(normalizedValue);

        return true;
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

    private void OnClickClose()
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
        if (masterValueText != null && masterSlider != null)
        {
            UpdateValueText(masterValueText, masterSlider.value);
        }

        if (bgmValueText != null && bgmSlider != null)
        {
            UpdateValueText(bgmValueText, bgmSlider.value);
        }

        if (sfxValueText != null && sfxSlider != null)
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