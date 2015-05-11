//-----------------------------
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
    using Assets.Scripts.Score;

    [Serializable]
    public class GameController : IPauseController
    {
        #region Private Members

        private GameTimeController m_gameTimeController;
        private ScoreController m_scoreController;
        private List<MoleController> m_moleControllers;
        private PlayerController m_playerController;

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
            InitializeGameTime();
        }

        public void Update()
        {
            if(IsPaused)
            {
                return;
            }

            UpdateGameStatus();
            UpdateScore();
            UpdatePlayer();
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

            if(!m_playerController.MoleHit)
            {
                return;
            }

            DetermineWhackedMole();
        }

        private void UpdateScore()
        {
            foreach (MoleController moleController in m_moleControllers)
            {
                if (!moleController.Hit)
                {
                    continue;
                }

                bool whackSuccessful = true;
                m_scoreController.IncreaseScore(moleController.ScoreValue);
                m_scoreController.RecordWhackAttempt(whackSuccessful);
                m_scoreController.IncrementMolesWhacked();

                m_playerController.UpdateStats(moleController.ScoreValue, whackSuccessful);
                return;
            }

            if (m_playerController.WhackTriggered)
            {
                bool whackSuccessful = false;
                m_scoreController.RecordWhackAttempt(whackSuccessful);

                m_playerController.UpdateStats(0, whackSuccessful);
            }
        }

        private void UpdateStars()
        {

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
                    moleController.Hit = false;
                }
            }
        }

        #endregion
    }
}
