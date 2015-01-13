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

        #endregion

        #region Delegates

        public delegate void TimerElapsedEvent();

        #endregion

        #region Public Properties

        public int IntervalInSecondsLeft
        {
            get
            {
                return IntervalInSeconds - m_stopwatch.Elapsed.Seconds;
            }
        }

        public int IntervalInSeconds
        {
            get;
            private set;
        }

        public bool AutoResetEnabled
        {
            get;
            private set;
        }

        public bool TimeHasElapsed
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
            IntervalInSeconds = intervalInSeconds;
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
            IntervalInSeconds = timeInSeconds;
            TimeHasElapsed = false;
        }

        public void StartTimer()
        {
            m_stopwatch.Start();
            TimeHasElapsed = false;
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

        public void ClearTimeElapsedNotification()
        {
            TimeHasElapsed = false;
        }

        public void Update()
        {
            if(!Active())
            {
                return;
            }

            if(IntervalInSecondsLeft > 0)
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
                TimeHasElapsed = true;
            }
        }

        #endregion
    }
}
