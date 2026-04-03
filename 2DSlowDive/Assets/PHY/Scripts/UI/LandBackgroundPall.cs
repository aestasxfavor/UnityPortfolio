using UnityEngine;

public class LandBackgroundPall : MonoBehaviour
{
    [SerializeField] private Vector2 moveDir = Vector2.right;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float range = 0.5f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * range;
        transform.position = startPos + (Vector3)moveDir * offset;
    }
}
