using TMPro;
using UnityEngine;

class TimerTextController : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private Timer timer;

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
    }

    void Update()
    {
        timerText.text = timer.GetTimerText();
    }
}