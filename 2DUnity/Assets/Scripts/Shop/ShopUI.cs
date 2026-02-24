
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Tab Buttons")]
    [SerializeField] private Button coinTabButton;
    [SerializeField] private Button shopTabButton;

    [Header("Tab Roots")]
    [SerializeField] private GameObject coinTabRoot;
    [SerializeField] private GameObject shopTabRoot;

    private void Awake()
    {
        coinTabButton.onClick.AddListener(OpenCoinTab);
        shopTabButton.onClick.AddListener(OpenShopTab);
    }

    // 상점 열릴 때 기본 상태
    public void Open()
    {
        OpenCoinTab();
    
    }

    public void OpenCoinTab()
    {
        Debug.Log("코인탭 전환");
        coinTabRoot.SetActive(true);
        shopTabRoot.SetActive(false);
    }

    public void OpenShopTab()
    {
        Debug.Log("상점탭 전환");
        coinTabRoot.SetActive(false);
        shopTabRoot.SetActive(true);
    }
}
