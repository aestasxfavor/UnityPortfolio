using UnityEngine;

public class CoinService : MonoBehaviour
{
    public static CoinService Instance { get; private set; }

    public int CurrentCoin { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    private void Start()
    {
        LoadCoin();
    }

    private void LoadCoin()
    {
        var save = SaveManager.Instance.GetCoinSaveData();
        CurrentCoin = save.currentCoin;
    }
    
    public void AddCoin(int amount)
    {
        CurrentCoin += amount;
        SaveCoin();
    }

    public bool SpendCoin(int amount)
    {
        if (CurrentCoin < amount) return false;
        CurrentCoin -= amount;
        SaveCoin();
        return true;
    }

    private void SaveCoin()
    {
        var save = SaveManager.Instance.GetCoinSaveData();
        save.currentCoin = CurrentCoin;

        SaveManager.Instance.RequestSave();
    }
}
