using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Vector2 movement;

    private void Update()
    {
        ProcessTranslation();
    }
    public void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
        Debug.Log($"Move Input : {movement}");
    }

    public void ProcessTranslation()
    {
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        transform.localPosition += move * speed * Time.deltaTime;
    }

    public void OnJump()
    {
        Debug.Log("Jump");
    }
}
