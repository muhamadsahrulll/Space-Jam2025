using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    private float remainingTime;
    public TMP_Text timerText;
    private bool running;

    void OnEnable()
    {
        //remainingTime = startTime;
        //running = true;
    }

    public void StartTimer(float time)
    {
        remainingTime = time;
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

        if (timerText != null)
        {
            timerText.text = $"Time: {remainingTime:0}";
        }
    }

    public void StopTimer() => running = false;
}