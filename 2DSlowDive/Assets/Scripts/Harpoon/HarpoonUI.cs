using UnityEngine;
using UnityEngine.UI;

public class HarpoonUI : MonoBehaviour, UIClosable
{
    [SerializeField] private PlayerAim playerAim;
    [SerializeField] private Outline normalHighlight;
    [SerializeField] private Outline tripleHighlight;

    [SerializeField] private GameObject tripleSlot;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (playerAim == null)
            playerAim = FindFirstObjectByType<PlayerAim>();

        if (playerAim == null)
            return;

        if (!SaveManager.Instance.HasHarpoonUpgrade())
            tripleSlot.SetActive(false);
        else
            tripleSlot.SetActive(true);

        if (playerAim.CurrentHarpoonType == HarpoonType.Normal)
        {
            normalHighlight.enabled = true;
            tripleHighlight.enabled = false;
        }
        else
        {
            normalHighlight.enabled = false;
            tripleHighlight.enabled = true;
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}