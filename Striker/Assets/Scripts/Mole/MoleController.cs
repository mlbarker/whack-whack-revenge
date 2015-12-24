//-----------------------------
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
        private IAttackController m_attackController;
        private Timer m_movementTimer;
        private Timer m_recoveryTimer;
        private Timer m_attackTimer;
        private IRandom m_random;
        private AnimationStateMachine m_asm;
        private int m_upCount;
        private int m_downCount;

        #endregion

        #region Public Properties

        public int MaxHealth
        {
            get;
            private set;
        }
        
        public int Health
        {
            get;
            private set;
        }

        public bool Idle
        {
            get
            {
                return m_status[MoleStatus.Idle];
            }
        }

        public bool Injured
        {
            get
            {
                return Health != MaxHealth && !Swoon;
            }
        }

        public bool InjuredAnimFinished
        {
            get;
            private set;
        }

        public bool Swoon
        {
            get
            {
                return Health < 1;
            }
        }

        public bool Attack
        {
            get
            {
                return m_status[MoleStatus.Attack];
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

        public int SecondsTillAttack
        {
            get;
            private set;
        }

        public bool CompletedCycle
        {
            get
            {
                return m_upCount + m_downCount == 2;
            }
        }

        public bool Active
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
            m_movementTimer.StopTimer();
            m_recoveryTimer.StopTimer();

            IsPaused = true;
        }

        public void OnGameResumed()
        {
            m_movementTimer.StartTimer();
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

        public int maxHealth;
        public int scoreValue;
        public int healthTickInSeconds;
        public int healthPerTick;
        public int maxSecondsDown;
        public int maxSecondsUp;
        public int secondsTillAttack;

        #endregion

        #region Public Methods

        public void Initialize()
        {
            MaxHealth = maxHealth;
            Health = maxHealth;
            ScoreValue = scoreValue;
            HealthTickInSeconds = healthTickInSeconds;
            HealthPerTick = healthPerTick;
            MaxSecondsDown = maxSecondsDown;
            MaxSecondsUp = maxSecondsUp;
            SecondsTillAttack = secondsTillAttack;

            InitializeRandom();
            InitializeTimers();
            InitializeStatus();
            InitializeASM();

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

            if(!Active)
            {
                StopAllTimers();
                ResetStatus();
                return;
            }

            UpdateHealth();
            UpdateStatus();
            UpdateAttackTimer();
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

        public void SetAttackController(IAttackController attackController)
        {
            if (attackController == null)
            {
                return;
            }

            m_attackController = attackController;
        }

        public void StartMole()
        {
            ResetStatus();

            int intervalInSeconds = m_random.RandomInt(1, MaxSecondsDown);
            m_movementTimer.SetTimer(intervalInSeconds);
            m_movementTimer.StartTimer();
        }

        public void StopMole()
        {
            // Set status to match in-hole variables
            m_movementTimer.StopTimer();
            m_recoveryTimer.StopTimer();
            
            if(m_attackTimer != null)
            {
                m_attackTimer.StopTimer();
            }

            ResetStatus();

            Health = MaxHealth;
        }

        public void SetActive(bool active)
        {
            Active = active;
        }
         
        public void TransitionInjuredToIdle()
        {

            m_status[MoleStatus.Idle] = true;

            // called to reset injured animation vars
            TriggerInjuredMoleMovement();
            StoppedMoving();
            InjuredAnimFinished = true;
        }

        public void ClearCycle()
        {
            m_upCount = 0;
            m_downCount = 0;
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

            if(m_movementTimer == null)
            {
                int intervalInSeconds = m_random.RandomInt(1, MaxSecondsDown);
                m_movementTimer = new Timer(intervalInSeconds, TriggerBasicMoleMovement);
            }

            if(m_attackController != null)
            { 
                if(m_attackTimer == null)
                {
                    m_attackTimer = new Timer(SecondsTillAttack, StartAttackSequence);
                }
            }

            m_movementTimer.StopTimer();
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
            m_status.Add(MoleStatus.Idle, false);
            m_status.Add(MoleStatus.Attack, false);
            m_status.Add(MoleStatus.Swoon, false);

            InjuredAnimFinished = true;
        }

        private void InitializeASM()
        {
            m_asm = new AnimationStateMachine();
        }

        private void AddToPauseManager()
        {
            PauseManager.Instance.Add(GetHashCode(), this);
        }

        private void StartAttackSequence()
        {
            if (!Swoon)
            {
                m_status[MoleStatus.Attack] = true;
            }
        }

        private void TriggerSwoonMoleMovement()
        {
            // downCount++;
            IsUp = false;
            IsMoving = true;
            m_status[MoleStatus.Attack] = false;
            m_movementController.MoveIntoHoleOnSwoon();
            m_downCount++;
        }

        private void TriggerBasicMoleMovement()
        {
            if(IsUp)
            {
                // TODO: should not set mole status here... this needs to change
                m_status[MoleStatus.Idle] = false;
                m_status[MoleStatus.Attack] = false;
                IsUp = false;
                IsMoving = true;
                m_movementController.MoveIntoHole();
                m_downCount++;
            }
            else
            {
                // upCount++;
                IsUp = true;
                IsMoving = true;
                m_movementController.MoveOutOfHole();
                m_upCount++;
            }
        }

        private void TriggerIdleMovement()
        {
            m_movementController.MoveToIdle();
        }

        private void TriggerAttackMovement()
        {
            IsMoving = true;
            m_status[MoleStatus.Attack] = true;
            m_status[MoleStatus.Idle] = false;
            m_attackController.Attack();
        }

        private void TriggerInjuredMoleMovement()
        {
            IsMoving = true;
            InjuredAnimFinished = false;
            m_movementController.MoveOnInjured();
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
                m_status[MoleStatus.Idle] = false;
            }

            if(Swoon && IsUp)
            {
                TriggerSwoonMoleMovement();
            }
            else if(!Swoon && IsUp && Hit)
            {
                TriggerInjuredMoleMovement();
            }
            else if(IsUp && !IsMoving && !Idle && !Attack)
            {
                m_status[MoleStatus.Idle] = true;
                TriggerIdleMovement();
            }
            else if(Attack)
            {
                // TriggerAttack(); - Make sure to set IsMoving to true inside of method
                TriggerAttackMovement();
            }

            if(IsUp)
            {
                m_status[MoleStatus.Recovering] = false;
            }
            else
            {
                m_status[MoleStatus.Recovering] = true;
                m_status[MoleStatus.Idle] = false;
            }

            if(Health == MaxHealth)
            {
                m_status[MoleStatus.Healthy] = true;
                m_status[MoleStatus.Injured] = false;
            }
        }

        private void UpdateMoveTimer()
        {
            if(GetMoleStatus(MoleStatus.Recovering) &&
               GetMoleStatus(MoleStatus.Injured) || 
               GetMoleStatus(MoleStatus.Attack))
            {
                m_movementTimer.StopTimer();
                m_movementTimer.ResetTimer();
            }

            if(GetMoleStatus(MoleStatus.Healthy) &&
               GetMoleStatus(MoleStatus.Recovering) &&
               !m_movementTimer.Active())
            {
                int intervalInSeconds = m_random.RandomInt(1, MaxSecondsDown);
                m_movementTimer.SetTimer(intervalInSeconds);
                m_movementTimer.StartTimer();
            }

            if(IsMoving)
            {
                m_movementTimer.StopTimer();
                m_movementTimer.ResetTimer();
            }

            if (IsUp && 
                Idle &&
                !Attack &&
                !IsMoving &&
                !m_movementTimer.Active())
            {
                int intervalInSeconds = m_random.RandomInt(1, MaxSecondsDown);
                m_movementTimer.SetTimer(intervalInSeconds);
                m_movementTimer.StartTimer();
            }

            m_movementTimer.Update();
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

        private void UpdateAttackTimer()
        {
            if(m_attackController == null)
            {
                return;
            }

            if(Idle &&
               IsUp &&
               m_movementTimer.TimeHasElapsed &&
               !IsMoving && 
               !m_attackTimer.Active())
            {
                m_attackTimer.StartTimer();

                // TODO: Don't want to pollute this attack timer
                // TODO: method with a different timer. Find a better way....
                m_movementTimer.StopTimer();
                m_movementTimer.ResetTimer();
            }

            if(GetMoleStatus(MoleStatus.Attack))
            {
                m_attackTimer.StopTimer();
                m_attackTimer.ResetTimer();
            }

            m_attackTimer.Update();
        }

        private void ResetStatus()
        {
            m_status[MoleStatus.Healthy] = true;
            m_status[MoleStatus.Injured] = false;
            m_status[MoleStatus.Recovering] = true;
            m_status[MoleStatus.Idle] = false;
            m_status[MoleStatus.Attack] = false;
            m_status[MoleStatus.Swoon] = false;
        }

        private void StopAllTimers()
        {
            m_movementTimer.StopTimer();
            m_movementTimer.ResetTimer();

            m_recoveryTimer.StopTimer();
            m_recoveryTimer.ResetTimer();

            if(m_attackController != null)
            {
                m_attackTimer.StopTimer();
                m_attackTimer.ResetTimer();
            }
        }

        private void ClearHit()
        {
            if(Hit)
            {
                Hit = false;
            }
        }

        internal void ClearAttack()
        {
            m_status[MoleStatus.Attack] = false;
            m_status[MoleStatus.Idle] = true;

            // TODO: There's gotta be a better way...
            m_movementTimer.StartTimer();
            m_attackTimer.StopTimer();
            m_attackTimer.ResetTimer();
        }

        #endregion
    }
}
