using UnityEngine;

public class Timer
{
    private float timeRemaining;
    private bool isRunning;

    public Timer(float initial)
    {
        timeRemaining = initial;
        isRunning = false;
    }

    public void Start()
    {
        isRunning = true;
    }

    public void Stop()
    {
        isRunning = false;
    }

    public void Reset(float newTime)
    {
        timeRemaining = newTime;
        isRunning = false;
    }

    public void Update()
    {
        if (isRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isRunning = false;
                // Trigger a timeout event or handle timeout
            }
        }
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public bool IsTimeUp()
    {
        return timeRemaining <= 0;
    }
    public float Timezero()
    {
        return 0.0f;
    }
    
}
