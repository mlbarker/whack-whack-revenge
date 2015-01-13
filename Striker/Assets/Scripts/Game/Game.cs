//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Game
{
    using UnityEngine;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Mole;
    using Assets.Scripts.Player;
    using Assets.Scripts.Utilities.Timers;

    public class Game : MonoBehaviour, IGameTimeController
    {
        #region Private Members

        private Timer m_gameTimer;

        #endregion

        #region Editor Values

        public GameController gameController;
        public Mole mole;
        public Player player;

        #endregion

        #region IGameTimerController Properties

        public int GameTimeSeconds
        {
            get
            {
                return m_gameTimer.IntervalInSeconds;
            }
        }

        public int GameTimeSecondsLeft
        {
            get
            {
                return m_gameTimer.IntervalInSecondsLeft;
            }
        }

        #endregion

        #region IGameTimeController Methods

        public void SetGameTime(int gameTimeSeconds)
        {
            if(m_gameTimer == null)
            {
                throw new TimerException();
            }

            m_gameTimer.SetTimer(gameTimeSeconds);
        }

        public void UpdateTime()
        {
            m_gameTimer.Update();
        }

        public void TimeUpCallback()
        {
            GameIsOver();
        }

        #endregion

        #region Unity Methods

        void Start()
        {
            Initialize();
        }

        void Update()
        {
            UpdateGame();
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            if(gameController == null)
            {
                throw new GameControllerException();
            }

            if(mole == null)
            {
                throw new MoleException();
            }

            if(player == null)
            {
                throw new PlayerException();
            }

            m_gameTimer = new Timer(gameController.gameTimeSeconds, GameIsOver);
            if(m_gameTimer == null)
            {
                throw new TimerException();
            }

            mole.Initialize();
            player.Initialize();
            gameController.SetGameTimeController(this);
            gameController.SetMoleController(mole.moleController);
            gameController.SetPlayerController(player.playerController);
            gameController.Initialize();
        }

        private void UpdateGame()
        {
            MoleWasWhacked();
            gameController.Update();
        }

        private void MoleWasWhacked()
        {            
            if(player.HitCollisionId == -1)
            {
                return;
            }

            if(mole.collider == null)
            {
                return;
            }

            if(player.HitCollisionId == mole.collider.GetInstanceID())
            {
                mole.moleController.Hit = true;
            }
        }

        private void GameIsOver()
        {

        }

        #endregion
    }
}
