using UnityEngine;

public class FishInventoryService : MonoBehaviour
{
    public static FishInventoryService Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance != null)
        {

            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void AddFish()
    {

    }

  
}
