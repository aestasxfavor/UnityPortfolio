using UnityEngine;

public class HighFallDetector : MonoBehaviour
{
    [Header("High Fall Check")]
    [SerializeField] private float highFallTime = 0.35f;
    [SerializeField] private float highFallSpeed = -7f;

    public float FallTimer { get; private set; }
    public float MinYSpeed { get; private set; }
    public bool IsHighFalling { get; private set; }

    private bool isTracking;

    public void StartFall(float currentYSpeed)
    {
        isTracking = true;
        FallTimer = 0f;
        MinYSpeed = currentYSpeed;
        IsHighFalling = false;

        Debug.Log("HighFallDetector StartFall");
    }

    public void TickFall(float deltaTime, float currentYSpeed)
    {
        if (!isTracking) return;

        FallTimer += deltaTime;

        if (currentYSpeed < MinYSpeed)
        {
            MinYSpeed = currentYSpeed;
        }

        if (!IsHighFalling &&
            FallTimer >= highFallTime &&
            MinYSpeed <= highFallSpeed)
        {
            IsHighFalling = true;
            Debug.Log("HighFallDetector isHighFalling true");
        }
    }

    public bool ConsumeHighFallResult()
    {
        bool result = IsHighFalling;
        ResetFall();
        return result;
    }

    public void ResetFall()
    {
        isTracking = false;
        FallTimer = 0f;
        MinYSpeed = 0f;
        IsHighFalling = false;
    }
}