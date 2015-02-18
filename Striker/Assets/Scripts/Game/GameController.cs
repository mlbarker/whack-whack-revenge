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
    public class GameController
    {
        #region Private Members

        private IGameTimeController m_gameTimeController;
        private IScoreController m_scoreController;

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
            get;
            private set;
        }

        #endregion

        #region Editor Values

        public IGameTimeController gameTimeController;
        public IScoreController scoreController;
        public PlayerController playerController;
        public MoleController[] moleControllers;

        #endregion

        #region Public Methods

        public void Initialize()
        {
            InitializeGameTime();
            InitializeScore();
            InitializePlayer();
            InitializeMoles();
        }

        public void Update()
        {
            UpdateGameStatus();
            UpdateScore();
            UpdatePlayer();
            UpdateMole();
            UpdateTime();
            GameTimeIsUp();
        }

        #endregion

        #region Private Methods

        private void InitializeGameTime()
        {
            m_gameTimeController = gameTimeController;
            if (m_gameTimeController == null)
            {
                throw new GameTimeControllerException();
            }

            m_gameTimeController.Initialize();
        }

        private void InitializeScore()
        {
            m_scoreController = scoreController;
            if (m_scoreController == null)
            {
                throw new ScoreControllerException();
            }
        }

        private void InitializePlayer()
        {
            m_playerController = playerController;
            if(m_playerController == null)
            {
                throw new PlayerControllerException();
            }

            m_playerController.Initialize();
        }

        private void InitializeMoles()
        {
            m_moleControllers = new List<MoleController>(moleControllers);
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
            CurrentScore += IncrementScoreOnHits();

            if(m_playerController.WhackTriggered)
            {
                //m_scoreController.HitPercentageUpdate();
            }
        }

        private void GameTimeIsUp()
        {
            if(m_gameTimeController.GameTimeSecondsLeft > 0)
            {
                return;
            }

            m_gameTimeController.TimeUpCallback();
            IsTimeUp = true;
        }

        private int IncrementScoreOnHits()
        {
            int score = 0;
            foreach (MoleController moleController in m_moleControllers)
            {
                if (!moleController.Hit)
                {
                    continue;
                }

                //m_scoreController.ScoreUpdate();
                ++score;
            }

            return score;
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
