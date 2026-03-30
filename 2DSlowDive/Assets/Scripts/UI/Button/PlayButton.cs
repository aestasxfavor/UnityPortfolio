using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Awake()
    {
        if(button == null)
        {
            button = GetComponent<Button>();
        }

        button.onClick.AddListener(OnClickPlay);
      
    }

    private void OnClickPlay()
    {
        SceneryManager.Instance.LoadScene("Ocean");
    }

}
