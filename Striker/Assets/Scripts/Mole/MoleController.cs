﻿//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Mole
{
    using System;
    using System.Collections.Generic;
    using Assets.Scripts.Game;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Utilities.Logger;
    using Assets.Scripts.Utilities.Random;
    using Assets.Scripts.Utilities.Timers;

    [Serializable]
    public class MoleController : IPauseController
    {
        #region Fields

        private Dictionary<MoleStatus, bool> m_status = new Dictionary<MoleStatus,bool>();
        private IMovementController m_movementController;
        private IHealthController m_healthController;
        private Timer m_timer;
        private Timer m_recoveryTimer;
        private IRandom m_random;

        #endregion

        #region Public Properties
        
        public int Health
        {
            get;
            private set;
        }

        public bool Injured
        {
            get
            {
                return Health < 1;
            }
        }

        public int ScoreValue
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

        public bool IsMoving
        {
            get;
            private set;
        }

        public int HealthTickInSeconds
        {
            get;
            private set;
        }

        public int HealthPerTick
        {
            get;
            private set;
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

        #region IPauseController Properties

        public bool IsPaused
        {
            get;
            private set;
        }

        #endregion

        #region IPauseController Methods

        public void OnGamePaused()
        {
            m_timer.StopTimer();
            m_recoveryTimer.StopTimer();

            IsPaused = true;
        }

        public void OnGameResumed()
        {
            m_timer.StartTimer();
            m_recoveryTimer.StartTimer();

            IsPaused = false;
        }

        #endregion

        #region Constructor

        public MoleController()
        {
        }

        #endregion

        #region Editor Values

        public int health;
        public int scoreValue;
        public int healthTickInSeconds;
        public int healthPerTick;
        public int maxSecondsDown;
        public int maxSecondsUp;

        #endregion

        #region Public Methods

        public void Initialize()
        {
            Health = health;
            ScoreValue = scoreValue;
            HealthTickInSeconds = healthTickInSeconds;
            HealthPerTick = healthPerTick;
            MaxSecondsDown = maxSecondsDown;
            MaxSecondsUp = maxSecondsUp;

            InitializeRandom();
            InitializeTimers();
            InitializeStatus();

            if(m_movementController == null)
            {
                return;
            }

            AddToPauseManager();
        }

        public void Update()
        {
            if(IsPaused)
            {
                return;
            }

            UpdateHealth();
            UpdateStatus();
            UpdateMoveTimer();
            UpdateRecoveryTimer();
            ClearHit();
        }

        public void DecrementHealth(int amount)
        {
            Health -= amount;
        }

        public void IncrementHealth(int amount)
        {
            Health += amount;
        }

        public bool GetMoleStatus(MoleStatus status)
        {
            return m_status[status];
        }

        public void StoppedMoving()
        {
            IsMoving = false;
        }

        public void SetMovementController(IMovementController movementController)
        {
            if(movementController == null)
            {
                return;
            }

            m_movementController = movementController;
        }

        public void SetHealthController(IHealthController healthController)
        {
            if(healthController == null)
            {
                return;
            }

            m_healthController = healthController;
        }

        public void StartMole()
        {
            m_timer.StartTimer();
        }

        public void TransitionInjuredToMoveIntoHole()
        {
            m_movementController.MoveIntoHole();
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
            if(m_recoveryTimer == null)
            {
                m_recoveryTimer = new Timer(HealthTickInSeconds, RecoverHealth);
            }

            if(m_timer == null)
            {
                int intervalInSeconds = m_random.RandomInt(1, MaxSecondsDown);
                m_timer = new Timer(intervalInSeconds, TriggerBasicMoleMovement);
            }

            m_timer.StopTimer();
            //m_timer.StartTimer();
        }

        private void InitializeStatus()
        {
            if(m_status.Count == (int)MoleStatus.Total)
            {
                return;
            }

            m_status.Add(MoleStatus.Healthy, true);
            m_status.Add(MoleStatus.Injured, false);
            m_status.Add(MoleStatus.Recovering, true);
        }

        private void AddToPauseManager()
        {
            PauseManager.Instance.Add(GetHashCode(), this);
        }

        private void TriggerInjuredMoleMovement()
        {
            m_movementController.MoveIntoHoleOnInjured();
            IsUp = false;
            IsMoving = true;
        }

        private void TriggerBasicMoleMovement()
        {
            if(IsUp)
            {
                IsUp = false;
                m_movementController.MoveIntoHole();
                IsMoving = true;
            }
            else
            {
                IsUp = true;
                m_movementController.MoveOutOfHole();
                IsMoving = true;
            }
        }

        private void RecoverHealth()
        {
            m_healthController.RecoverHealth();
        }

        private void UpdateHealth()
        {
            if(Hit)
            {
                m_healthController.AdjustHealth();
            }
        }

        private void UpdateStatus()
        {
            if(Hit)
            {
                m_status[MoleStatus.Healthy] = false;
                m_status[MoleStatus.Injured] = true;
            }

            if(Injured && IsUp)
            {
                TriggerInjuredMoleMovement();
            }

            if(IsUp)
            {
                m_status[MoleStatus.Recovering] = false;
            }
            else
            {
                m_status[MoleStatus.Recovering] = true;
            }

            if(Health == health)
            {
                m_status[MoleStatus.Healthy] = true;
                m_status[MoleStatus.Injured] = false;
            }
        }

        private void UpdateMoveTimer()
        {
            if(GetMoleStatus(MoleStatus.Recovering) &&
               GetMoleStatus(MoleStatus.Injured))
            {
                m_timer.StopTimer();
                m_timer.ResetTimer();
            }

            if(GetMoleStatus(MoleStatus.Healthy) &&
               GetMoleStatus(MoleStatus.Recovering) &&
               !m_timer.Active())
            {
                int intervalInSeconds = m_random.RandomInt(MaxSecondsDown);
                m_timer.SetTimer(intervalInSeconds);
                m_timer.StartTimer();
            }

            if(IsMoving)
            {
                m_timer.StopTimer();
                m_timer.ResetTimer();
            }

            if (IsUp && !m_timer.Active() && !IsMoving)
            {
                int intervalInSeconds = m_random.RandomInt(MaxSecondsDown);
                m_timer.SetTimer(intervalInSeconds);
                m_timer.StartTimer();
            }

            m_timer.Update();
        }

        private void UpdateRecoveryTimer()
        {
            if (GetMoleStatus(MoleStatus.Healthy))
            {
                m_recoveryTimer.StopTimer();
                m_recoveryTimer.ResetTimer();
            }

            if(GetMoleStatus(MoleStatus.Recovering) && 
               !m_recoveryTimer.Active() && 
               !GetMoleStatus(MoleStatus.Healthy))
            {
                m_recoveryTimer.StartTimer();
            }

            if(!GetMoleStatus(MoleStatus.Recovering))
            {
                m_recoveryTimer.StopTimer();
                m_recoveryTimer.ResetTimer();
            }

            m_recoveryTimer.Update();
        }

        private void ClearHit()
        {
            if(Hit)
            {
                Hit = false;
            }
        }

        #endregion
    }
}
