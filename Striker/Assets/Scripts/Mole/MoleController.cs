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
        private Timer m_inHoleTimer;
        private Timer m_outOfHoleTimer;

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

        public int MaxSecondsInHole
        {
            get;
            private set;
        }

        public int MaxSecondsOutOfHole
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
        public int maxSecondsInHole;
        public int maxSecondsOutOfHole;

        #endregion

        #region Public Methods

        public void Initialize()
        {
            Health = health;
            MaxSecondsInHole = maxSecondsInHole;
            MaxSecondsOutOfHole = maxSecondsOutOfHole;
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
            m_inHoleTimer = new Timer(MaxSecondsInHole, m_movementController.MoveOutOfHole);
            m_outOfHoleTimer = new Timer(MaxSecondsOutOfHole, m_movementController.MoveIntoHole);

            if(IsUp)
            {
                m_outOfHoleTimer.StartTimer();
            }
            else
            {
                m_inHoleTimer.StartTimer();
            }
        }

        private void UpdateHealth()
        {
            if(Hit && Health == 0)
            {
                m_healthController.AdjustHealth();

                Hit = false;
                IsUp = false;

                m_inHoleTimer.StartTimer();
            }

            if(Hit && Health > 0)
            {
                m_healthController.AdjustHealth();

                Hit = false;
            }
        }

        private void UpdateTimers()
        {
            if(IsUp && !m_outOfHoleTimer.Active())
            {
                m_inHoleTimer.StartTimer();

                ToggleUp();
            }
            else if(!IsUp && !m_inHoleTimer.Active())
            {
                m_outOfHoleTimer.StartTimer();

                ToggleUp();
            }

            m_inHoleTimer.Update();
            m_outOfHoleTimer.Update();
        }

        #endregion
    }
}
