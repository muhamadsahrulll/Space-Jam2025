using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float startTime = 30f;
    private float remainingTime;
    public TMP_Text timerText; // Ensure this is assigned in the Inspector
    private bool running;

    void OnEnable()
    {
        remainingTime = startTime;
        running = true;
    }

    void Update()
    {
        if (!running) return;

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0)
        {
            remainingTime = 0;
            running = false;
            GameManager.Instance.OnTimeUp();
        }

        if (timerText != null) // Add a null check to avoid errors
        {
            timerText.text = $"Time: {remainingTime:0}";
        }
        else
        {
            Debug.LogError("TimerText is not assigned!");
        }
    }

    public void StopTimer() => running = false;
}