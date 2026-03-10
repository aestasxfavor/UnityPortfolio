using UnityEngine;
using UnityEngine.UI;
using System;

public class PopupManager : MonoBehaviour
{
    [Header("ÆË¾÷ ÇÁ¸®ÆÕ")]
    [SerializeField] private GameObject buyCheckCanvas;
    [SerializeField] private GameObject exchangeCoinCanvas;
    [SerializeField] private GameObject hasHarpoonCanvas;
    [SerializeField] private GameObject fallCoinCanvas;

    private GameObject buyCheckInstance;
    private GameObject exchangeInstance;
    private GameObject hasHarpoonInstance;
    private GameObject fallCoinInstance;

    private Action yesAction;

    private void SetConfirmButtons(GameObject popup)
    {
        var buttons = popup.GetComponentsInChildren<Button>(true);

        foreach (var btn in buttons)
        {
            switch (btn.name)
            {
                case "Yes":
                    btn.onClick.AddListener(OnClickYes);
                    break;

                case "No":
                    btn.onClick.AddListener(() => popup.SetActive(false));
                    break;
            }
        }
    }

    private void OnClickYes()
    {
        Debug.Log("Yes Clicked");

        var action = yesAction;
        CloseAll();
        action?.Invoke();
    }

    private void BuyCheckPopup()
    {
        if (buyCheckInstance == null && buyCheckCanvas != null)
        {
            buyCheckInstance = Instantiate(buyCheckCanvas);
            buyCheckInstance.SetActive(false);
            SetConfirmButtons(buyCheckInstance);
        }
    }

    private void ExchangePopup()
    {
        if (exchangeInstance == null && exchangeCoinCanvas != null)
        {
            exchangeInstance = Instantiate(exchangeCoinCanvas);
            exchangeInstance.SetActive(false);
            SetConfirmButtons(exchangeInstance);
        }
    }

    private void HasHarpoonPopup()
    {
        if (hasHarpoonInstance == null && hasHarpoonCanvas != null)
        {
            hasHarpoonInstance = Instantiate(hasHarpoonCanvas);
            hasHarpoonInstance.SetActive(false);

            var buttons = hasHarpoonInstance.GetComponentsInChildren<Button>(true);

            foreach (var btn in buttons)
            {
                btn.onClick.AddListener(() => hasHarpoonInstance.SetActive(false));
            }
        }
    }

    private void CoinFailPopup()
    {
        if (fallCoinInstance == null && fallCoinCanvas != null)
        {
            fallCoinInstance = Instantiate(fallCoinCanvas);
            fallCoinInstance.SetActive(false);

            var buttons = fallCoinInstance.GetComponentsInChildren<Button>(true);

            foreach (var btn in buttons)
            {
                btn.onClick.AddListener(() => fallCoinInstance.SetActive(false));
            }
        }
    }

    public void ShowBuyCheck(Action action)
    {
        CloseAll();
        yesAction = action;

        BuyCheckPopup();
        buyCheckInstance.SetActive(true);
    }

    public void ShowExchange(Action action)
    {
        CloseAll();
        yesAction = action;

        ExchangePopup();
        exchangeInstance.SetActive(true);
    }

    public void ShowHasHarpoon()
    {
        CloseAll();

        HasHarpoonPopup();
        hasHarpoonInstance.SetActive(true);
    }

    public void ShowCoinFail()
    {
        CloseAll();

        CoinFailPopup();
        fallCoinInstance.SetActive(true);
    }

    public void CloseAll()
    {
        if (buyCheckInstance != null) buyCheckInstance.SetActive(false);
        if (exchangeInstance != null) exchangeInstance.SetActive(false);
        if (hasHarpoonInstance != null) hasHarpoonInstance.SetActive(false);
        if (fallCoinInstance != null) fallCoinInstance.SetActive(false);

        yesAction = null;
    }
}