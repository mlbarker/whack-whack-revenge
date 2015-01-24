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

    public class Game : MonoBehaviour, IGameTimeController, IScoreController
    {
        #region Private Members

        private Timer m_gameTimer;

        #endregion

        #region Public Properties

        public int Score
        {
            get;
            private set;
        }

        public float WhackPercentage
        {
            get;
            private set;
        }

        #endregion

        #region Editor Values

        public GameController gameController;
        public Mole [] moles;
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
            if(!m_gameTimer.Active() && !m_gameTimer.TimeHasElapsed)
            {
                m_gameTimer.StartTimer();
            }

            m_gameTimer.Update();
        }

        public void TimeUpCallback()
        {
            GameIsOver();
        }

        #endregion

        #region IScoreController Methods

        public void ScoreUpdate()
        {
            ++Score;
        }

        public void HitPercentageUpdate()
        {
            WhackPercentage = (float)Score / (float)player.WhackAttempts;
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

            if(player == null)
            {
                throw new PlayerException();
            }

            m_gameTimer = new Timer(gameController.gameTimeSeconds, GameIsOver);
            if(m_gameTimer == null)
            {
                throw new TimerException();
            }

            player.Initialize();
            gameController.SetGameTimeController(this);
            gameController.SetScoreController(this);
            InitializeMoles();
            gameController.SetPlayerController(player.playerController);
            gameController.Initialize();
        }

        private void InitializeMoles()
        {
            if (moles == null)
            {
                throw new MoleException();
            }

            foreach(Mole mole in moles)
            {
                mole.Initialize();
                gameController.SetMoleController(mole.moleController);
            }
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

            foreach(Mole mole in moles)
            {
                if(mole.collider2D == null)
                {
                    continue;
                }

                int hitCollision2dId = mole.collider2D.GetInstanceID();
                if(player.HitCollisionId == hitCollision2dId)
                {
                    mole.moleController.Hit = true;
                    break;
                }
            }

            player.ClearHitCollisionId();
        }

        private void GameIsOver()
        {

        }

        #endregion
    }
}
