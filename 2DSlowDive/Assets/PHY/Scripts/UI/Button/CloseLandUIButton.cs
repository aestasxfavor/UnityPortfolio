using UnityEngine;
using UnityEngine.UI;

public class CloseLandUIButton : MonoBehaviour
{
    private LandUIManager manager;

    public void Init(LandUIManager _manager)
    {
        manager = _manager;
        GetComponent<Button>().onClick.AddListener(manager.CloseAllLandUI);
    }
}
