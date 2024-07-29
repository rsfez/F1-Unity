using Models;
using TMPro;
using UnityEngine;

namespace Controllers.UI
{
    public class LapTextController : MonoBehaviour
    {
        private TextMeshProUGUI _lapText;
        private Timer _timer;

        private void Awake()
        {
            _lapText = GetComponent<TextMeshProUGUI>();
            _timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
            ;
        }

        private void Update()
        {
            _lapText.text = "Lap: " + _timer.GetCurrentLap() + "/" + _timer.GetTotalLaps();
        }
    }
}