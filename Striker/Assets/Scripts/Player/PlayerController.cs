//-----------------------------
// ImperfectlyCoded © 2014-2015
//-----------------------------

namespace Assets.Scripts.Player
{
    using System;
    using System.Collections.Generic;
    using Assets.Scripts.Game;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Persistence;
    using Assets.Scripts.Score;
    using Assets.Scripts.Stars;
    using Assets.Scripts.Utilities.Timers;

    [Serializable]
    public class PlayerController : IPauseController
    {
        #region Private Members

        private IHitController m_hitController;
        private IInputController m_inputController;
        
        #endregion

        #region Public Properties

        public int MaxHealth
        {
            get;
            private set;
        }

        public int CurrentHealth
        {
            get;
            private set;
        }

        public bool WhackTriggered
        {
            get;
            private set;
        }

        // either a mole or projectile was hit
        public bool ObjectHit
        {
            get;
            private set;
        }

        public int StarsCollected
        {
            get;
            private set;
        }

        public int LifetimeScore
        {
            get;
            private set;
        }

        public int LifetimeWhacks
        {
            get;
            private set;
        }

        public int LifetimeWhackAttempts
        {
            get;
            private set;
        }

        public float LifetimeWhackPercentage
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
            IsPaused = true;
        }

        public void OnGameResumed()
        {
            IsPaused = false;
        }

        #endregion

        #region Editor Values

        public int maxHealth; 

        #endregion

        #region Public Methods

        public void Initialize()
        {
            AddToPauseManager();
            LoadLifetimeStats();
            InitializeHealth();
        }

        public void Update()
        {
            if(IsPaused)
            {
                return;
            }

            UpdateInput();
            UpdateHit();
        }

        public void UpdateStats(ScoreController scoreController, int score, bool successfulWhack)
        {
            scoreController.IncreaseScore(score);
            scoreController.RecordWhackAttempt(successfulWhack);
        }

        public void UpdateLifetimeStats(ScoreController scoreController,  int starsAchieved)
        {
            LifetimeScore += scoreController.Score;
            LifetimeWhackAttempts += scoreController.WhackAttempts;
            LifetimeWhacks += scoreController.Whacks;
            StarsCollected += starsAchieved;
        }

        public void SetInputController(IInputController inputController)
        {
            m_inputController = inputController;
            if(m_inputController == null)
            {
                throw new InputControllerException();
            }
        }

        public void SetHitController(IHitController hitController)
        {
            m_hitController = hitController;
            if(m_hitController == null)
            {
                throw new HitControllerException();
            }
        }

        public void DecrementHealth(int value)
        {
            CurrentHealth -= value;
        }

        #endregion

        #region Private Methods

        private void InitializeHealth()
        {
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
        }

        private void AddToPauseManager()
        {
            PauseManager.Instance.Add(GetHashCode(), this);
        }

        private void UpdateInput()
        {
            if (!EndGameManager.Instance.IsEndGameActive && !EndGameManager.Instance.IsEndGameTimeDone)
            {
                WhackTriggered = m_inputController.AttackButton();
            }
            else
            {
                int test = 0;
                test++;
            }
        }

        private void UpdateHit()
        {
            if(!WhackTriggered)
            {
                ObjectHit = false;
                return;
            }

            ObjectHit = m_hitController.HitDetected();
        }

        private void LoadLifetimeStats()
        {
            int playerKey = PersistentManager.PlayerKey;

            LifetimeScore = PersistentManager.Instance.GetValue(playerKey, playerKey, DataIndex.LifetimeScore);
            LifetimeWhackAttempts = PersistentManager.Instance.GetValue(playerKey, playerKey, DataIndex.LifetimeWhacks);
            LifetimeWhacks = PersistentManager.Instance.GetValue(playerKey, playerKey, DataIndex.LifetimeWhackAttempts);
            StarsCollected = PersistentManager.Instance.GetValue(playerKey, playerKey, DataIndex.StarsCollected);
        }

        #endregion
    }
}
