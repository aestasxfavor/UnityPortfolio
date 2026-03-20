using UnityEngine;
using UnityEngine.SceneManagement;

public enum BGMType { Title, Ocean, Land }

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    #region Audio References (오디오 참조)
    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip titleBGM;
    [SerializeField] private AudioClip oceanBGM;
    [SerializeField] private AudioClip landBGM;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip harpoonFireSFX;
    [SerializeField] private AudioClip swimSFX;
    [SerializeField] private AudioClip buttonSFX;
    [SerializeField] private AudioClip unlockButtonSFX;
    [SerializeField] private AudioClip waterSplashSFX;

    private AudioSource loopSFXSource;
    #endregion

    #region Volume Settings (볼륨 설정값)
    [Header("Default Sound")]
    [SerializeField, Range(0f, 1f)] private float defaultMasterVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float defaultBgmVolume = 0.8f;
    [SerializeField, Range(0f, 1f)] private float defaultSfxVolume = 1f;

    [Header("Current Sound")]
    [SerializeField, Range(0f, 1f)] private float masterVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float bgmVolume = 0.8f;
    [SerializeField, Range(0f, 1f)] private float sfxVolume = 1f;
    #endregion

    #region Save Keys (저장 키)
    private const string MASTER_VOLUME_KEY = "Sound_MasterVolume";
    private const string BGM_VOLUME_KEY = "Sound_BgmVolume";
    private const string SFX_VOLUME_KEY = "Sound_SfxVolume";
    #endregion

    #region Properties (프로퍼티)
    public float MasterVolume => masterVolume;
    public float BgmVolume => bgmVolume;
    public float SfxVolume => sfxVolume;
    #endregion

    #region Singleton & Lifecycle (싱글톤 및 생명주기)
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitLoopSFXSource();
        LoadVolumeSettings();
        ApplyVolumes();
    }

    private void Start()
    {
        PlayBGMByScene(SceneManager.GetActiveScene().name);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    #endregion

    #region Initialization (초기화)
    private void InitLoopSFXSource()
    {
        if (loopSFXSource != null) return;

        loopSFXSource = gameObject.AddComponent<AudioSource>();
        loopSFXSource.loop = true;
        loopSFXSource.playOnAwake = false;
        loopSFXSource.spatialBlend = 0f;
    }
    #endregion

    #region Scene BGM Control (씬별 배경음 제어)
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMByScene(scene.name);
        ApplyVolumes();
    }

    public void PlayBGMByScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Title":
                PlayBGM(BGMType.Title);
                break;

            case "Ocean":
                PlayBGM(BGMType.Ocean);
                break;

            case "Land":
                PlayBGM(BGMType.Land);
                break;
        }
    }

    public void PlayBGM(BGMType type)
    {
        if (bgmSource == null) return;

        AudioClip nextClip = null;

        switch (type)
        {
            case BGMType.Title:
                nextClip = titleBGM;
                break;

            case BGMType.Ocean:
                nextClip = oceanBGM;
                break;

            case BGMType.Land:
                nextClip = landBGM;
                break;
        }

        if (nextClip == null) return;

        if (bgmSource.clip == nextClip && bgmSource.isPlaying) return;

        bgmSource.Stop();
        bgmSource.clip = nextClip;
        bgmSource.loop = true;
        ApplyVolumes();
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource == null) return;
        bgmSource.Stop();
    }
    #endregion

    #region SFX Playback (효과음 재생)
    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.PlayOneShot(clip, Mathf.Clamp01(volumeScale));
    }

    public void PlayHarpoonFireSFX()
    {
        PlaySFX(harpoonFireSFX);
    }

    public void PlayButtonSFX()
    {
        PlaySFX(buttonSFX);
    }

    public void PlayUnlockButtonSFX()
    {
        PlaySFX(unlockButtonSFX);
    }

    public void PlayWaterSplashSFX()
    {
        PlaySFX(waterSplashSFX);
    }

    public void PlaySwimSFX()
    {
        if (swimSFX == null) return;

        InitLoopSFXSource();

        if (!loopSFXSource.isPlaying)
        {
            loopSFXSource.clip = swimSFX;
            loopSFXSource.volume = masterVolume * sfxVolume;
            loopSFXSource.Play();
        }
    }

    public void StopSwimSFX()
    {
        if (loopSFXSource != null && loopSFXSource.isPlaying)
        {
            loopSFXSource.Stop();
        }
    }
    #endregion

    #region Volume Control (볼륨 제어)
    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp01(value);
        ApplyVolumes();
    }

    public void SetBgmVolume(float value)
    {
        bgmVolume = Mathf.Clamp01(value);
        ApplyVolumes();
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
        ApplyVolumes();
    }

    public void ResetToDefaultVolumes()
    {
        masterVolume = defaultMasterVolume;
        bgmVolume = defaultBgmVolume;
        sfxVolume = defaultSfxVolume;
        ApplyVolumes();
    }

    public void ApplyVolumes()
    {
        if (bgmSource != null)
        {
            bgmSource.volume = masterVolume * bgmVolume;
        }

        if (sfxSource != null)
        {
            sfxSource.volume = masterVolume * sfxVolume;
        }

        if (loopSFXSource != null)
        {
            loopSFXSource.volume = masterVolume * sfxVolume;
        }
    }
    #endregion

    #region Volume Save & Load (볼륨 저장 및 불러오기)
    public void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, masterVolume);
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, bgmVolume);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, sfxVolume);
        PlayerPrefs.Save();
    }

    public void LoadVolumeSettings()
    {
        masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, defaultMasterVolume);
        bgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, defaultBgmVolume);
        sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, defaultSfxVolume);
    }
    #endregion
}