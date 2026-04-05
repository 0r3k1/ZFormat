using System;
using System.Diagnostics;

namespace ZFormat.Core {
    public class ZTimer {
        private Stopwatch _stopwatch;
        private double _interval;

        public event Action OnTick;
        public bool Enabled { get; set; }
        public bool AutoReset { get; set; } = true;

        public double Interval {
            get => _interval;
            set => _interval = value;
        }

        public ZTimer(double intervalMs) {
            _interval = intervalMs;
            _stopwatch = new Stopwatch();
        }

        public void Start() {
            Enabled = true;
            _stopwatch.Restart();
        }

        public void Stop() {
            Enabled = false;
            _stopwatch.Stop();
        }

        // Este método lo llamaremos dentro del loop de ZApp
        public void Update() {
            if(!Enabled)
                return;

            if(_stopwatch.Elapsed.TotalMilliseconds >= _interval) {
                OnTick?.Invoke();

                if(AutoReset) {
                    _stopwatch.Restart();
                } else {
                    Stop();
                }
            }
        }
    }
}