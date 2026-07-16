using TMPro;
using UnityEngine;

public class DiveResultUI : MonoBehaviour
{
    [SerializeField] private TMP_Text caughtCountText;
    [SerializeField] private TMP_Text newDiscoveryText;

    public void ShowResult(int caughtCount, int newDiscoveryCount)
    {
        gameObject.SetActive(true);

        caughtCountText.text = $"회수한 표본 {caughtCount}개";

        bool hasNewDiscovery = newDiscoveryCount > 0;

        newDiscoveryText.gameObject.SetActive(hasNewDiscovery);
        if (hasNewDiscovery)
        {
            newDiscoveryText.text += $" 신규 발견 {newDiscoveryCount}종";
        }
    }

    public void HideResult()
    {
        gameObject.SetActive(false);
    }
}
