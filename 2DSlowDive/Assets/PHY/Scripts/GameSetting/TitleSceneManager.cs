using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Land";

    private bool isLoading = false;

    private void Update()
    {
        if (isLoading) return;

        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            isLoading = true;
            SceneManager.LoadScene(nextSceneName);
        }
    }
}