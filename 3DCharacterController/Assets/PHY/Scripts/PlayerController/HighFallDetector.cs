using UnityEngine;

public class HighFallDetector : MonoBehaviour
{
    [Header("High Fall Check")]
    [SerializeField] private float highFallTime = 0.35f;
    [SerializeField] private float highFallSpeed = -4f;

    public float FallTimer { get; private set; }
    public float LowYSpeed { get; private set; }
    public bool IsHighFall { get; private set; }

    private bool isTracking;

    public void StartFall(float currentYSpeed)
    {
        isTracking = true;
        FallTimer = 0f;
        LowYSpeed = currentYSpeed;
        IsHighFall = false;

        Debug.Log("HighFallDetector StartFall");
    }

    public void CheckHighFall(float deltaTime, float currentYSpeed)
    {
        if (!isTracking) return;

        FallTimer += deltaTime;

        if (currentYSpeed < LowYSpeed)
        {
            LowYSpeed = currentYSpeed;
        }

        if (!IsHighFall && FallTimer >= highFallTime && LowYSpeed <= highFallSpeed)
        {
            IsHighFall = true;
            Debug.Log("HighFallDetector isHighFalling true");
        }
    }

    public bool GetResultAndReset()
    {
        bool result = IsHighFall;
        ResetFall();
        return result;
    }

    public void ResetFall()
    {
        isTracking = false;
        FallTimer = 0f;
        LowYSpeed = 0f;
        IsHighFall = false;
    }
}