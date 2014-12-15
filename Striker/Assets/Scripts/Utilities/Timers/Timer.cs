//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Utilities.Timers
{
    using System.Diagnostics;

    public class Timer : ITimer
    {
        #region Private Members

        private Stopwatch m_stopwatch;
        private TimerElapsedEvent m_elapsedEvent;
        private int m_intervalInSeconds;

        #endregion

        #region Delegates

        public delegate void TimerElapsedEvent();

        #endregion

        #region Public Properties

        public int IntervalInSeconds
        {
            get
            {
                return m_intervalInSeconds - m_stopwatch.Elapsed.Seconds;
            }
        }

        public bool AutoResetEnabled
        {
            get;
            private set;
        }

        #endregion

        #region Constructor

        private Timer()
        { 
        }

        public Timer(int intervalInSeconds, TimerElapsedEvent timerElapsedEvent)
        {
            m_intervalInSeconds = intervalInSeconds;
            m_elapsedEvent = timerElapsedEvent;

            m_stopwatch = new Stopwatch();
        }

        #endregion

        #region ITimer methods

        public bool Active()
        {
            return m_stopwatch.IsRunning;
        }

        public void SetTimer(int timeInSeconds)
        {
            m_intervalInSeconds = timeInSeconds;
        }

        public void StartTimer()
        {
            m_stopwatch.Start();
        }

        public void StopTimer()
        {
            m_stopwatch.Stop();
        }

        public void ResetTimer()
        {
            m_stopwatch.Reset();
        }

        public void AutoResetTimer(bool autoReset)
        {
            AutoResetEnabled = autoReset;
        }

        public void Update()
        {
            if(IntervalInSeconds > 0)
            {
                return;
            }

            ElapsedEventCallback();
            StopTimer();
            ResetTimer();
            AutoReset();
        }

        #endregion

        #region Private Methods

        private void AutoReset()
        {
            if(AutoResetEnabled)
            {
                StartTimer();
            }
        }

        private void ElapsedEventCallback()
        {
            if(Active())
            {
                m_elapsedEvent();
            }
        }

        #endregion
    }
}
