using UnityEngine;
using UnityEngine.SceneManagement;

public enum BGMType { Title, Sea, Land }

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("BGM")]
    public AudioSource bgmSource;
    public AudioClip titleBGM;
    public AudioClip seaBGM;
    public AudioClip landBGM;

    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip harpoonFireSFX;
    public AudioClip swimSFX;
    public AudioClip ButtonSFX;
    public AudioClip UnLockButtonSFX;
    public AudioClip waterSplashSFX;

    private AudioSource loopSFXSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayBGMByScene(SceneManager.GetActiveScene().name);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGMByScene(scene.name);
    }

    public void PlayBGMByScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Title": PlayBGM(BGMType.Title); break;
            case "Ocean": PlayBGM(BGMType.Sea); break;
            case "Land": PlayBGM(BGMType.Land); break;
        }
    }

    public void PlayBGM(BGMType type)
    {
        if (bgmSource == null) return;

        if (bgmSource.isPlaying)
            bgmSource.Stop();

        switch (type)
        {
            case BGMType.Title: bgmSource.clip = titleBGM; break;
            case BGMType.Sea: bgmSource.clip = seaBGM; break;
            case BGMType.Land: bgmSource.clip = landBGM; break;
        }

        if (bgmSource.clip == null) return;

        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayHarpoonFireSFX()
    {
        PlaySFX(harpoonFireSFX);
    }

    public void PlaySwimSFX()
    {
       // Debug.Log("[Sound] PlaySwimSFX called");
        if (swimSFX == null) return;

        if (loopSFXSource == null)
        {
            loopSFXSource = gameObject.AddComponent<AudioSource>();
            loopSFXSource.loop = true;
            loopSFXSource.playOnAwake = false;
        }

        if (!loopSFXSource.isPlaying)
        {
            loopSFXSource.clip = swimSFX;
            loopSFXSource.Play();
        }
    }

    public void StopSwimSFX()
    {
        if (loopSFXSource != null && loopSFXSource.isPlaying)
            loopSFXSource.Stop();
    }
}
