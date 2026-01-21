using UnityEngine;

public class ActiveWatch : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.LogError($"[WATCH] {name} ENABLED\n{System.Environment.StackTrace}");
    }

    private void OnDisable()
    {
        Debug.LogError($"[WATCH] {name} DISABLED\n{System.Environment.StackTrace}");
    }
}
