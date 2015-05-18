﻿//-----------------------------
// ImperfectlyCoded © 2014-2015
//-----------------------------

namespace Assets.Scripts.Player
{
    using System;
    using Assets.Scripts.Game;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Score;
    using Assets.Scripts.Utilities.Timers;

    [Serializable]
    public class PlayerController : IPauseController
    {
        #region Private Members

        private IHitController m_hitController;
        private IInputController m_inputController;

        private ScoreController m_scoreController;
        
        #endregion

        #region Public Properties

        public bool WhackTriggered
        {
            get;
            private set;
        }

        public bool MoleHit
        {
            get;
            private set;
        }

        public int TotalScore
        {
            get
            {
                return m_scoreController.Score;
            }
        }

        public int TotalWhacks
        {
            get
            {
                return m_scoreController.Whacks;
            }
        }

        public int TotalWhackAttempts
        {
            get
            {
                return m_scoreController.WhackAttempts;
            }
        }

        public float TotalWhackPercentage
        {
            get
            {
                return m_scoreController.WhackPercentage;
            }
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

        #endregion

        #region Public Methods

        public void Initialize()
        {
            InitializeScore();
            AddToPauseManager();
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

        public void UpdateStats(int score, bool successfulWhack)
        {
            m_scoreController.IncreaseScore(score);
            m_scoreController.RecordWhackAttempt(successfulWhack);
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

        #endregion

        #region Private Methods

        private void InitializeScore()
        {
            m_scoreController = new ScoreController();
        }

        private void AddToPauseManager()
        {
            PauseManager.Instance.Add(typeof(PlayerController), this);
        }

        private void UpdateInput()
        {
            WhackTriggered = m_inputController.AttackButton();
        }

        private void UpdateHit()
        {
            if(!WhackTriggered)
            {
                MoleHit = false;
                return;
            }

            MoleHit = m_hitController.HitDetected();
        }

        #endregion
    }
}