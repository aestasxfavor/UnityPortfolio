using UnityEngine;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour, UIClosable
{
    [SerializeField] private OxygenConfigSO oxygenConfig;

    [SerializeField] private float currentOxygen;
    [SerializeField] private Slider oxygenSlider;

    private bool isActive = true;

    void Start()
    {
        currentOxygen = oxygenConfig.maxOxygen;
        oxygenSlider.maxValue = oxygenConfig.maxOxygen;
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


            if (SceneFlowManager.Instance != null)
                SceneFlowManager.Instance.ChangeScene(SceneType.Land);
        }
    }

    public void ResetOxygen()
    {
        currentOxygen = oxygenConfig.maxOxygen;
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
