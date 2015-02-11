//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Game
{
    using System;
    using System.Collections.Generic;
    using Assets.Scripts.Game;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Mole;
    using Assets.Scripts.Player;

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

        public int gameTimeSeconds;

        #endregion

        #region Public Methods

        public void Initialize()
        {
            if(m_gameTimeController == null)
            {
                throw new GameControllerException();
            }

            if(m_scoreController == null)
            {
                throw new ScoreControllerException();
            }

            if(m_playerController == null)
            {
                throw new PlayerControllerException();
            }

            m_gameTimeController.SetGameTime(gameTimeSeconds);

            m_playerController.Initialize();
            InitializeMoleControllers();
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

        public void SetGameTimeController(IGameTimeController gameTimeController)
        {
            m_gameTimeController = gameTimeController;
        }

        public void SetScoreController(IScoreController scoreController)
        {
            m_scoreController = scoreController;
        }

        public void SetMoleController(MoleController moleController)
        {
            if(m_moleControllers == null)
            {
                m_moleControllers = new List<MoleController>();
            }

            m_moleControllers.Add(moleController);
        }

        public void SetPlayerController(PlayerController playerController)
        {
            m_playerController = playerController;
        }

        #endregion

        #region Private Methods

        private void InitializeMoleControllers()
        {
            foreach(MoleController moleController in m_moleControllers)
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
                m_scoreController.HitPercentageUpdate();
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

                m_scoreController.ScoreUpdate();
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
