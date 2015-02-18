//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.GameTime
{
    using System;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Utilities.Timers;

    [Serializable]
    public class GameTimeController : IGameTimeController
    {
        #region Private Members

        private Timer m_gameTimer;
        private TimeElapsedEvent m_elapsedEvent;

        #endregion

        #region Delegates

        public delegate void TimeElapsedEvent();

        #endregion

        #region Public Properties

        public bool TimeElapsed
        {
            get
            {
                return m_gameTimer.TimeHasElapsed;
            }
        }

        #endregion

        #region IGameTimeController Properties

        public int GameTimeSeconds 
        { 
            get
            {
                return m_gameTimer.IntervalInSeconds;
            }
        }

        public int GameTimeSecondsLeft 
        { 
            get
            {
                return m_gameTimer.IntervalInSecondsLeft;
            }
        }

        #endregion

        #region IGameTimeController Methods

        public void SetGameTime(int seconds)
        {
            m_gameTimer.SetTimer(seconds);
        }

        public void UpdateTime()
        {
            m_gameTimer.Update();
        }

        public void TimeUpCallback()
        {
            if(m_elapsedEvent == null)
            {
                throw new GameTimeControllerException();
            }

            m_elapsedEvent();
        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            m_gameTimer = new Timer(0, TimeUpCallback);
        }

        public void SetTimeElapsedCallback(TimeElapsedEvent timeEvent)
        {
            m_elapsedEvent += timeEvent;
        }

        public void Reset()
        {
            m_gameTimer.ResetTimer();
        }

        public void Start()
        {
            m_gameTimer.StartTimer();
        }

        public void Stop()
        {
            m_gameTimer.StopTimer();
        }

        #endregion
    }
}
