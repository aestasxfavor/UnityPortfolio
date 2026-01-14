using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private bool isLockedButton = false;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        if (SoundManager.Instance == null) return;

        if (isLockedButton)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.UnLockButtonSFX);
        else
            SoundManager.Instance.PlaySFX(SoundManager.Instance.ButtonSFX);
    }
}
