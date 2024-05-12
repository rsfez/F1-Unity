using UnityEngine;

namespace Models
{
    public class Timer : MonoBehaviour
    {
        public int currentLap = 1;
        private short _multiplier = 1;
        private float _timer;
        private bool _timerRunning;
        private int _totalLaps;

        private void Update()
        {
            if (_timerRunning) _timer += Time.deltaTime * 1000 * _multiplier;
        }

        public void StartTimer()
        {
            _timerRunning = true;
        }

        public void PauseTimer()
        {
            _timerRunning = false;
        }

        public bool IsRunning()
        {
            return _timerRunning;
        }

        public long GetTime()
        {
            return (long)_timer;
        }

        public string GetTimerText()
        {
            var totalSeconds = (long)_timer / 1000;
            var minutes = (int)(totalSeconds / 60);
            var seconds = (int)(totalSeconds % 60);
            var milliseconds = (int)(_timer % 1000);
            return $"{minutes:00}:{seconds:00}.{milliseconds:000}";
        }

        public void SetMultiplier(short multiplier)
        {
            _multiplier = multiplier;
        }

        public int GetTotalLaps()
        {
            return _totalLaps;
        }

        public void TryUpdateTotalLaps(int totalLaps)
        {
            if (totalLaps > _totalLaps) _totalLaps = totalLaps;
        }
    }
}