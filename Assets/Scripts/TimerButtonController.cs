using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerButtonController : MonoBehaviour
{
    private Timer timer;
    private Button button;
    private TextMeshProUGUI buttonText;

    void Start()
    {
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        if (timer.IsRunning())
        {
            timer.PauseTimer();
            buttonText.text = "Start";
        }
        else
        {
            timer.StartTimer();
            buttonText.text = "Pause";
        }
    }
}
