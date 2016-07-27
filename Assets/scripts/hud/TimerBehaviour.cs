using UnityEngine;
using UnityEngine.UI;
using System;

namespace hud
{
    [RequireComponent(typeof(Text))]
    public class TimerBehaviour : MonoBehaviour
    {
        private Text timerText;
        public bool ClockRunning { get; set; }
        private float time;
        public float Time
        {
            get
            {
                return time;
            }
            private set
            {
                time = value;
                TimeSpan t = TimeSpan.FromSeconds(time);
                timerText.text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
            }
        }

        void Start()
        {
            timerText = GetComponent<Text>();
            Time = 0;
        }

        void Update()
        {
            if (ClockRunning)
                Time += UnityEngine.Time.deltaTime;
        }
    }
}