//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Game
{
    using System;
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

        // TODO: in the future, these need to be interfaces
        private MoleController m_moleController;
        private PlayerController m_playerController;

        #endregion

        #region Public Properties

        public bool IsTimeUp
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

            if(m_moleController == null)
            {
                throw new MoleControllerException();
            }

            if(m_playerController == null)
            {
                throw new PlayerControllerException();
            }

            m_gameTimeController.SetGameTime(gameTimeSeconds);

            m_playerController.Initialize();
            m_moleController.Initialize();
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
            m_moleController = moleController;
        }

        public void SetPlayerController(PlayerController playerController)
        {
            m_playerController = playerController;
        }

        #endregion

        #region Private Methods

        private void UpdateTime()
        {
            m_gameTimeController.UpdateTime();
        }

        private void UpdateMole()
        {
            m_moleController.UpdateStatus();
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

            if(!m_moleController.IsUp)
            {
                return;
            }

            m_moleController.Hit = true;
        }

        private void UpdateScore()
        {
            if(m_moleController.Hit)
            {
                m_scoreController.ScoreUpdate();
            }

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

        #endregion
    }
}
