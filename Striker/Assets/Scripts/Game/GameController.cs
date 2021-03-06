﻿//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Game
{
    using System;
    using System.Collections.Generic;
    using Assets.Scripts.Game;
    using Assets.Scripts.GameTime;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Mole;
    using Assets.Scripts.Player;
    using Assets.Scripts.Projectile;
    using Assets.Scripts.Score;

    [Serializable]
    public class GameController : IPauseController
    {
        #region Private Members

        private GameTimeController m_gameTimeController;
        private ScoreController m_scoreController;
        private List<MoleController> m_moleControllers;
        private PlayerController m_playerController;
        private List<Projectile> m_projectiles;

        #endregion

        #region Public Properties

        public bool IsTimeUp
        {
            get;
            private set;
        }

        public int CurrentScore
        {
            get
            {
                return m_scoreController.Score;
            }
        }

        public float CurrentPercentage
        {
            get
            {
                return m_scoreController.WhackPercentage;
            }
        }

        public int CurrentMolesWhacked
        {
            get
            {
                return m_scoreController.MolesWhacked;
            }
        }

        public int CurrentWhackAttempts
        {
            get
            {
                return m_scoreController.WhackAttempts;
            }
        }

        public int GameTimeSeconds
        {
            get;
            private set;
        }

        public int GameTimeSecondsLeft
        {
            get
            {
                return m_gameTimeController.GameTimeSecondsLeft;
            }
        }

        #endregion

        #region Editor Values
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
            m_gameTimeController.Stop();
            IsPaused = true;
        }

        public void OnGameResumed()
        {
            m_gameTimeController.Start();
            IsPaused = false;
        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            InitializeScore();
            InitializePlayer();
            InitializeMoles();
            InitializeProjectiles();
            InitializeGameTime();
            AddToPauseManager();
        }

        // 
        public void UpdatePlayerStatus()
        {
            UpdatePlayer();
        }

        public void Update()
        {
            if(IsPaused)
            {
                return;
            }

            UpdatePlayer();
            UpdateGameStatus();
            UpdateScore();
            UpdateMole();
            UpdateTime();
            GameTimeIsUp();
        }

        public void SetGameTime(int seconds)
        {
            if(m_gameTimeController == null)
            {
                m_gameTimeController = new GameTimeController();
                m_gameTimeController.Initialize();
            }

            GameTimeSeconds = seconds;
            m_gameTimeController.SetGameTime(GameTimeSeconds);
        }

        public void SetOnGameEndCallback(GameTimeController.TimeElapsedEvent gameEndCallback)
        {
            if (m_gameTimeController == null)
            {
                m_gameTimeController = new GameTimeController();
                m_gameTimeController.Initialize();
            }

            m_gameTimeController.SetTimeElapsedCallback(gameEndCallback);
        }

        public void AddPlayerController(PlayerController playerController)
        {
            m_playerController = playerController;
        }

        public void AddMoleController(MoleController moleController)
        {
            if (m_moleControllers == null)
            {
                m_moleControllers = new List<MoleController>();
            }

            m_moleControllers.Add(moleController);
        }

        public void StartGame()
        {
            m_gameTimeController.Start();
        }

        public void StopGame()
        {
            m_gameTimeController.Stop();
        }

        public void EndGameStatUpdate(int starsAchieved)
        {
            UpdatePlayerStats(starsAchieved);
        }

        public void GetProjectilesList(List<Projectile> projectiles)
        {
            if(projectiles.Count != 0)
            {
                m_projectiles.Clear();
                m_projectiles.AddRange(projectiles);
            }
        }

        #endregion

        #region Private Methods

        private void InitializeGameTime()
        {
            if (m_gameTimeController == null)
            {
                throw new GameTimeControllerException();
            }
        }

        private void InitializeScore()
        {
            m_scoreController = new ScoreController();
            if (m_scoreController == null)
            {
                throw new ScoreControllerException();
            }
        }

        private void InitializePlayer()
        {
            if(m_playerController == null)
            {
                throw new PlayerControllerException();
            }

            m_playerController.Initialize();
        }

        private void InitializeMoles()
        {
            foreach (MoleController moleController in m_moleControllers)
            {
                if (moleController == null)
                {
                    throw new MoleControllerException();
                }

                moleController.Initialize();
            }
        }

        private void InitializeProjectiles()
        {
            m_projectiles = new List<Projectile>();
        }

        private void AddToPauseManager()
        {
            PauseManager.Instance.Add(GetHashCode(), this);
        }

        private void UpdateTime()
        {
            m_gameTimeController.UpdateTime();
        }

        private void UpdateMole()
        {
            foreach(MoleController moleController in m_moleControllers)
            {
                moleController.Update();
            }
        }

        private void UpdatePlayer()
        {
            m_playerController.Update();
        }

        private void UpdateGameStatus()
        {
            if(!m_playerController.WhackTriggered)
            {
                return;
            }

            if(!m_playerController.ObjectHit)
            {
                return;
            }

            DetermineWhackedMole();
        }

        private void UpdateScore()
        {
            // UpdateScoreMoles()
            foreach (MoleController moleController in m_moleControllers)
            {
                if (!moleController.Hit)
                {
                    continue;
                }

                bool whackSuccessful = true;
                m_scoreController.IncrementMolesWhacked();
                m_playerController.UpdateStats(m_scoreController, moleController.ScoreValue, whackSuccessful);
                return;
            }
            // UpdateScoreProjectile()
            foreach(Projectile projectile in m_projectiles)
            {
                if (projectile == null)
                {
                    continue;
                }

                if (!projectile.Hit)
                {
                    continue;
                }

                bool whackSuccessful = true;
                m_playerController.UpdateStats(m_scoreController, 0, whackSuccessful);
                return;
            }
            // UpdateScoreMiss()
            if (m_playerController.WhackTriggered)
            {
                bool whackSuccessful = false;
                m_scoreController.RecordWhackAttempt(whackSuccessful);
                m_playerController.UpdateStats(m_scoreController, 0, whackSuccessful);
            }
        }

        private void UpdatePlayerStats(int starsAchieved)
        {
            m_playerController.UpdateLifetimeStats(m_scoreController, starsAchieved);
        }

        private void GameTimeIsUp()
        {
            if(m_gameTimeController.GameTimeSecondsLeft > 0)
            {
                return;
            }

            m_gameTimeController.TimeUpCallback();
            m_gameTimeController.Stop();
            IsTimeUp = true;
        }

        private void DetermineWhackedMole()
        {
            foreach (MoleController moleController in m_moleControllers)
            {
                if (!moleController.IsUp && moleController.Hit)
                {
                    //moleController.Hit = false;
                }
            }
        }

        #endregion
    }
}
