using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float parallaxFactor = 0.5f;
    private Vector2 startPos;

    void Start()
    {
        if (!cam) cam = Camera.main.transform;
        startPos = transform.position;
    }

    void LateUpdate()
    {
        Vector2 camPos = cam.position;
        transform.position = startPos + (camPos * parallaxFactor);
    }
}
