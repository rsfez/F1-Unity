using Models;
using TMPro;
using UnityEngine;

public class TimerTextController : MonoBehaviour
{
    private Timer _timer;
    private TextMeshProUGUI _timerText;

    private void Awake()
    {
        _timerText = GetComponent<TextMeshProUGUI>();
        _timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
    }

    private void Update()
    {
        _timerText.text = _timer.GetTimerText();
    }
}