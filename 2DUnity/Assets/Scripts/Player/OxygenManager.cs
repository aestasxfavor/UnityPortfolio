using UnityEngine;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour
{
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

            
            SceneFlowManager.Instance.OnOxygenDepleted();
        }
    }

    public void ResetOxygen()
    {
        currentOxygen = maxOxygen;
        oxygenSlider.value = currentOxygen;
        isActive = true;
    }
}
