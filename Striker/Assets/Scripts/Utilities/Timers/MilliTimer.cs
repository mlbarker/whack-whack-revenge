//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Utilities.Timers
{
    using System.Threading;

    public class MilliTimer : ITimer
    {
        #region Private Members

        private System.Threading.Timer m_timer;
        private TimerElapsedEvent m_elapsedEvent;
        private int m_intervalInMilliseconds;
        private bool m_active;

        #endregion

        #region Delegates

        public delegate void TimerElapsedEvent();

        #endregion

        #region Constructor

        private MilliTimer()
        {
        }

        public MilliTimer(int intervalInMilliseconds, TimerElapsedEvent elapsedEvent)
        {
            m_intervalInMilliseconds = intervalInMilliseconds;
            m_elapsedEvent = elapsedEvent;
        }

        #endregion

        #region Public Methods

        public bool Active()
        {
            return m_active;
        }

        public void SetTimer(int intervalInMilliseconds)
        {
            if (m_timer == null)
            {
                m_timer = new System.Threading.Timer(TimerCallback);
            }

            m_intervalInMilliseconds = intervalInMilliseconds;
        }

        public void StartTimer()
        {
            m_timer.Change(m_intervalInMilliseconds, Timeout.Infinite);
            m_active = true;
        }

        public void StopTimer()
        {
            m_timer.Dispose();
            m_active = false;
        }

        public void ResetTimer()
        {
            // I think this will explode by using this but not sure.
            m_timer.Dispose();
            m_active = false;

            // might need to set timer here
            // SetTimer(m_intervalForMilliseconds);
        }

        public void AutoResetTimer(bool autoReset)
        {

        }

        public void Update()
        {

        }

        #endregion

        #region Private Methods

        private void TimerCallback(object state)
        {
            m_elapsedEvent();
        }

        #endregion
    }
}
