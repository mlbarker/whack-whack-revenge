//-----------------------------
// ImperfectlyCoded © 2014-2015
//-----------------------------

namespace Assets.Scripts.Game
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Assets.Scripts.Level;
    using Assets.Scripts.Mole;
    using Assets.Scripts.Player;
    using Assets.Scripts.Score;
    using Assets.Scripts.Utilities.Timers;

    public class Game : MonoBehaviour
    {
        #region Private Members

        private GameController m_gameController;
        private StarController m_starController;
        private LevelManager m_levelManager;

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

        public bool StartGameCalled
        {
            get;
            set;
        }

        public bool DisplayGameResults
        { 
            get; 
            private set; 
        }

        public bool DisplayObjectives
        {
            get;
            set;
        }

        public List<string> Objectives
        {
            get
            {
                return m_starController.Objectives;
            }
        }

        public int StarsAchievedCount
        {
            get
            {
                return m_starController.StarsAchievedCount;
            }
        }

        #endregion

        #region Editor Values

        public Mole [] moles;
        public Player player;

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
            InitializeStars();
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
            DisplayObjectives = true;
            StartGameCalled = false;

            m_levelManager = LevelManager.Instance;
            int gameTimeInSeconds = m_levelManager.SelectedLevelInfo.levelTimeInSeconds;

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

        private void InitializeStars()
        {
            m_starController = new StarController(m_gameController);
            m_starController.Initialize();
        }

        private bool OnDisplayObjectives()
        {
            if (DisplayObjectives)
            {
                OnGamePaused();
                return true;
            }

            if (StartGameCalled)
            {
                OnGameResumed();

                StartGameCalled = false;
            }

            return false;
        }

        private void OnGamePaused()
        {
            PauseManager.Instance.GamePaused();
        }

        private void OnGameResumed()
        {
            PauseManager.Instance.GameResumed();
        }

        private void UpdateGame()
        {
            if(OnDisplayObjectives())
            {
                return;
            }

            UpdateMoleWasWhacked();
            UpdateGameController();
            UpdateStarsAchievements();
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

        private void UpdateStarsAchievements()
        {
            m_starController.UpdateStarsAchievements();
        }

        private void GameIsOver()
        {
            DisplayGameResults = true;
        }

        #endregion
    }
}
