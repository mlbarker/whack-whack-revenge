//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Mole
{
    using System;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Utilities.Timers;

    [Serializable]
    public class MoleController
    {
        #region Fields

        private IMovementController m_movementController;
        private IHealthController m_healthController;
        private Timer m_upTimer;
        private Timer m_downTimer;

        #endregion

        #region Public Properties
        
        public int Health
        {
            get;
            private set;
        }

        public bool IsUp
        {
            get;
            private set;
        }

        public bool Hit
        {
            get;
            set;
        }

        public int MaxSecondsDown
        {
            get;
            private set;
        }

        public int MaxSecondsUp
        {
            get;
            private set;
        }

        #endregion

        #region Constructor

        public MoleController()
        {
        }

        #endregion

        #region Editor Values

        public int health = 1;
        public int maxSecondsDown;
        public int maxSecondsUp;

        #endregion

        #region Public Methods

        public void Initialize()
        {
            Health = health;
            MaxSecondsDown = maxSecondsDown;
            MaxSecondsUp = maxSecondsUp;

            if(m_movementController == null)
            {
                return;
            }

            if(m_upTimer == null)
            {
                m_upTimer = new Timer(MaxSecondsUp, m_movementController.MoveIntoHole);
            }
            else
            {
                m_upTimer.SetTimer(MaxSecondsUp);
            }

            if (m_downTimer == null)
            {
                m_downTimer = new Timer(MaxSecondsDown, m_movementController.MoveOutOfHole);
            }
            else
            {
                m_downTimer.SetTimer(MaxSecondsDown);
            }
        }

        public void UpdateStatus()
        {
            UpdateHealth();
            UpdateTimers();
        }

        public void ToggleUp()
        {
            IsUp = !IsUp;
        }

        public void DecrementHealth(int amount)
        {
            Health -= amount;
        }

        public void RestoreHealth()
        {
            Health = health;
        }

        public void SetMovementController(IMovementController movementController)
        {
            if(movementController == null)
            {
                return;
            }

            m_movementController = movementController;
            InitializeTimers();
        }

        public void SetHealthController(IHealthController healthController)
        {
            if(healthController == null)
            {
                return;
            }

            m_healthController = healthController;
        }

        #endregion

        #region Private Methods

        private void InitializeTimers()
        {
            if (m_upTimer == null)
            {
                m_upTimer = new Timer(MaxSecondsUp, m_movementController.MoveIntoHole);
            }

            if (m_downTimer == null)
            {
                m_downTimer = new Timer(MaxSecondsDown, m_movementController.MoveOutOfHole);
            }

            if(IsUp)
            {
                m_upTimer.StartTimer();
            }
            else
            {
                m_downTimer.StartTimer();
            }
        }

        private void UpdateHealth()
        {
            if(Hit && Health > 0)
            {
                m_healthController.AdjustHealth();

                Hit = false;
            }

            if (Health == 0)
            {
                Hit = false;
                IsUp = false;

                m_movementController.MoveIntoHole();
                m_upTimer.StopTimer();
                m_upTimer.ResetTimer();
                m_downTimer.StartTimer();
            }
        }

        private void UpdateTimers()
        {
            if(IsUp && !m_upTimer.Active())
            {
                m_downTimer.StartTimer();

                ToggleUp();
            }
            else if(!IsUp && !m_downTimer.Active())
            {
                m_upTimer.StartTimer();

                ToggleUp();
            }

            m_upTimer.Update();
            m_downTimer.Update();
            CheckIntervalsForTimers();
        }

        private void CheckIntervalsForTimers()
        {
            if(m_downTimer.TimeHasElapsed)
            {
                IsUp = true;
                m_downTimer.ClearTimeElapsedNotification();

                // comment out the below code for unit test
                m_upTimer.StartTimer();
                return;
            }

            if(m_upTimer.TimeHasElapsed)
            {
                IsUp = false;
                m_upTimer.ClearTimeElapsedNotification();

                // comment out the below code for unit test
                m_downTimer.StartTimer();
            }
        }

        #endregion
    }
}
