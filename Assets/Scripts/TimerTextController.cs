using Models;
using TMPro;
using UnityEngine;

public class TimerTextController : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private Timer timer;

    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
    }

    private void Update()
    {
        timerText.text = timer.GetTimerText();
    }
}