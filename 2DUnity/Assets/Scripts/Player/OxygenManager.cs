using UnityEngine;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour, UIClosable
{
    // SO로 나중에 빼기
    [SerializeField] private float maxOxygen = 99f;
    [SerializeField] private float currentOxygen;
    [SerializeField] private Slider oxygenSlider;

    private bool isActive = true;

    void Start()
    {
        currentOxygen = maxOxygen;
        oxygenSlider.maxValue = maxOxygen;
        oxygenSlider.value = currentOxygen;
    }

    void Update()
    {
        if (!isActive) return;

        currentOxygen -= Time.deltaTime;
        oxygenSlider.value = currentOxygen;

        if (currentOxygen <= 0)
        {
            currentOxygen = 0;
            isActive = false;

            Debug.Log("[OxygenManager] Oxygen Depleted");

            if (SceneFlowManager.Instance == null)
            {
                Debug.LogError("[OxygenManager] SceneFlowManager.Instance == null");
            }
            else
            {
                Debug.Log("[OxygenManager] ChangeScene(Land) 호출");
                SceneFlowManager.Instance.ChangeScene(SceneType.Land);
            }


            SceneFlowManager.Instance.ChangeScene(SceneType.Land);
        }
    }

    public void ResetOxygen()
    {
        currentOxygen = maxOxygen;
        oxygenSlider.value = currentOxygen;
        isActive = true;
    }

    public void Close()
    {
        // 대부분 UI는 Canvas 밑에 자식으로 있기 때문에 확실하게 끄려면
        // transform.root를 붙여서 꺼줘야 한다.
        transform.root.gameObject.SetActive(false);
    }
}
