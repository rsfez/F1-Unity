using UnityEngine;

namespace Models
{
    public class Timer : MonoBehaviour
    {
        private float timer;
        private short mutiplicator = 1;
        private bool timerRunning;

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
            var totalSeconds = (long)timer / 1000;
            var minutes = (int)(totalSeconds / 60);
            var seconds = (int)(totalSeconds % 60);
            var milliseconds = (int)(timer % 1000);
            return $"{minutes:00}:{seconds:00}.{milliseconds:000}";
        }

        public void SetMulitiplicator(short mutiplicator)
        {
            this.mutiplicator = mutiplicator;
        }
    }
}
