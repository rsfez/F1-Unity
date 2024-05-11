using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.UI
{
    public class TimerButtonController : MonoBehaviour
    {
        private Button _button;
        private TextMeshProUGUI _buttonText;
        private Timer _timer;

        private void Awake()
        {
            _timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
            _buttonText = GetComponentInChildren<TextMeshProUGUI>();
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            if (_timer.IsRunning())
            {
                _timer.PauseTimer();
                _buttonText.text = "Start";
            }
            else
            {
                _timer.StartTimer();
                _buttonText.text = "Pause";
            }
        }
    }
}