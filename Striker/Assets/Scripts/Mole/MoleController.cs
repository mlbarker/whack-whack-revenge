//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Mole
{
    using System;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Utilities.Random;
    using Assets.Scripts.Utilities.Timers;

    [Serializable]
    public class MoleController
    {
        #region Fields

        private IMovementController m_movementController;
        private IHealthController m_healthController;
        private Timer m_upTimer;
        private Timer m_downTimer;
        private IRandom m_random;

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

            InitializeRandom();

            if(m_movementController == null)
            {
                return;
            }

            if(m_upTimer == null)
            {
                int intervalInSeconds = m_random.RandomInt(MaxSecondsUp);
                m_upTimer = new Timer(intervalInSeconds, m_movementController.MoveIntoHole);
            }
            else
            {
                int intervalInSeconds = m_random.RandomInt(MaxSecondsUp);
                m_upTimer.SetTimer(intervalInSeconds);
            }

            if (m_downTimer == null)
            {
                int intervalInSeconds = m_random.RandomInt(MaxSecondsDown);
                m_downTimer = new Timer(intervalInSeconds, m_movementController.MoveOutOfHole);
            }
            else
            {
                int intervalInSeconds = m_random.RandomInt(MaxSecondsDown);
                m_downTimer.SetTimer(intervalInSeconds);
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

        public void SetRandomObject(IRandom random)
        {
            if(random == null)
            {
                return;
            }

            m_random = random;
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

        private void InitializeRandom()
        {
            if(m_random == null)
            {
                m_random = new Utilities.Random.Random();
            }
        }

        private void InitializeTimers()
        {
            InitializeRandom();

            if (m_upTimer == null)
            {
                int intervalInSeconds = m_random.RandomInt(1, MaxSecondsUp);
                m_upTimer = new Timer(intervalInSeconds, m_movementController.MoveIntoHole);
            }

            if (m_downTimer == null)
            {
                int intervalInSeconds = m_random.RandomInt(1, MaxSecondsDown);
                m_downTimer = new Timer(intervalInSeconds, m_movementController.MoveOutOfHole);
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

                int intervalInSeconds = m_random.RandomInt(MaxSecondsUp);
                m_upTimer.StopTimer();
                m_upTimer.ResetTimer();
                m_upTimer.SetTimer(intervalInSeconds);
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
                
                int intervalInSeconds = m_random.RandomInt(MaxSecondsDown);
                m_downTimer.ClearTimeElapsedNotification();
                m_downTimer.SetTimer(intervalInSeconds);

                m_upTimer.StartTimer();
                return;
            }

            if(m_upTimer.TimeHasElapsed)
            {
                IsUp = false;

                int intervalInSeconds = m_random.RandomInt(MaxSecondsUp);
                m_upTimer.ClearTimeElapsedNotification();
                m_upTimer.SetTimer(intervalInSeconds);

                m_downTimer.StartTimer();
            }
        }

        #endregion
    }
}
