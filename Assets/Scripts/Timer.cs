using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timer = 0;
    private short mutiplicator = 1;
    private bool timerRunning = false;

    void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime * 1000 * mutiplicator;
        }
    }

    public void StartTimer()
    {
        timerRunning = true;
    }

    public void PauseTimer()
    {
        timerRunning = false;
    }

    public bool IsRunning()
    {
        return timerRunning;
    }

    public long GetTime()
    {
        return (long)timer;
    }

    public string GetTimerText()
    {
        long totalSeconds = (long)timer / 1000;
        int minutes = (int)(totalSeconds / 60);
        int seconds = (int)(totalSeconds % 60);
        int milliseconds = (int)(timer % 1000);
        return string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
    }

    public void SetMulitiplicator(short mutiplicator)
    {
        this.mutiplicator = mutiplicator;
    }
}
