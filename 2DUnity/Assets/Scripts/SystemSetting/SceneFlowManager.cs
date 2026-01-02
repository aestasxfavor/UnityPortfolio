using System.Collections;
using UnityEngine;

public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager Instance;

    private bool isRunning = false;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnOxygenDepleted()
    {
        if (isRunning) return;
        isRunning = true;
        StartCoroutine(OxygenDepleteRoutine());
    }

    private IEnumerator OxygenDepleteRoutine()
    {
        Debug.Log("[SceneFlow] Oxygen Deplete Start");

        // TODO: Ocean UI 종료 (임시 처리, UI Manager로 이전 예정)

        var oceanUI = GameObject.Find("InventoryCanvas(Clone)");
        if(oceanUI != null) Destroy(oceanUI);

        var oxygenUI = GameObject.Find("OxygenUI(Clone)");
        if(oxygenUI != null) Destroy(oxygenUI);

        yield return new WaitForSeconds(0.2f);

        // TODO: Inventory Sea → Land 처리
        yield return new WaitForSeconds(0.5f);

        // TODO: Fade 연출
        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.GoToFadeScene();
    }
}
