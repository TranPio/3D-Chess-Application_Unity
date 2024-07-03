using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RadialCountdown : MonoBehaviour
{
    [Header("Image")]
    [SerializeField] private Image img;

    [Header("Logic")]
    private float lastStart;
    private float duration;
    private bool isActive;

    [Header("End")]
    [SerializeField] private UnityEvent onEnd;

    public void Update()
    {
        if (isActive)
            UpdateRadial();
    }
    private void UpdateRadial()
    {
        // Get the amount time left
        float ratio = 1 - ((Time.time - lastStart) / duration);
        img.fillAmount = ratio;


        // Invoke a callback when we're done
        if (ratio <= 0)
        {
            isActive = false;
            onEnd?.Invoke();
        }
    }

    public void StartCountdown(float seconds)
    {
        lastStart = Time.time;
        isActive = true;
        duration = seconds;
        UpdateRadial();
    }

    public float GetTimeLeft()
    {
        return ((Time.time - lastStart) / duration);
    }
}
