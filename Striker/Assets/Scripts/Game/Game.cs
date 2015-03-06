//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Game
{
    using UnityEngine;
    using Assets.Scripts.Mole;
    using Assets.Scripts.Player;
    using Assets.Scripts.Score;
    using Assets.Scripts.Utilities.Timers;

    public class Game : MonoBehaviour
    {
        #region Private Members

        private GameController m_gameController;

        #endregion

        #region Public Properties

        public int Score
        {
            get
            {
                return m_gameController.CurrentScore;
            }
        }

        public float WhackPercentage
        {
            get
            {
                return m_gameController.CurrentPercentage;
            }
        }

        public int GameTimeSecondsLeft
        {
            get
            {
                return m_gameController.GameTimeSecondsLeft;
            }
        }

        public bool DisplayGameResults
        { 
            get; 
            private set; 
        }

        #endregion

        #region Editor Values

        public Mole [] moles;
        public Player player;
        public int gameTimeInSeconds;

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
            InitializeMoles();
            InitializePlayer();
            InitializeGame();
        }

        private void InitializeMoles()
        {
            if (moles == null)
            {
                throw new MoleException();
            }

            foreach (Mole mole in moles)
            {
                mole.Initialize();
            }
        }

        private void InitializePlayer()
        {
            if (player == null)
            {
                throw new PlayerException();
            }

            player.Initialize();
        }

        private void InitializeGame()
        {
            DisplayGameResults = false;

            m_gameController = new GameController();
            m_gameController.SetGameTime(gameTimeInSeconds);
            m_gameController.SetOnGameEndCallback(GameIsOver);
            m_gameController.AddPlayerController(player.playerController);

            foreach(Mole mole in moles)
            {
                m_gameController.AddMoleController(mole.moleController);
            }

            m_gameController.Initialize();
        }

        private void UpdateGame()
        {
            UpdateMoleWasWhacked();
            UpdateGameController();
        }

        private void UpdateMoleWasWhacked()
        {
            if(DisplayGameResults)
            {
                return;
            }

            if (player.HitCollisionId == -1)
            {
                return;
            }

            foreach (Mole mole in moles)
            {
                if (mole.GetComponent<Collider2D>() == null)
                {
                    continue;
                }

                int hitCollision2dId = mole.GetComponent<Collider2D>().GetInstanceID();
                if (player.HitCollisionId == hitCollision2dId)
                {
                    mole.moleController.Hit = true;
                    break;
                }
            }

            player.ClearHitCollisionId();
        }

        private void UpdateGameController()
        {
            if (DisplayGameResults)
            {
                return;
            }

            m_gameController.Update();
        }

        private void GameIsOver()
        {
            DisplayGameResults = true;
        }

        #endregion
    }
}
