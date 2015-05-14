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
        private Dictionary<LevelStarType, List<int>> m_levelStars = new Dictionary<LevelStarType, List<int>>();
        private Dictionary<LevelStarType, List<int>> m_levelStarStats = new Dictionary<LevelStarType, List<int>>();
        private Dictionary<LevelStarType, bool> m_levelStarsAchieved = new Dictionary<LevelStarType, bool>();
        private List<string> m_objectives = new List<string>();

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
            //LevelInfo levelInfo = m_levelManager.SelectedLevelInfo;

            //foreach (LevelStarInfo starInfo in levelInfo.levelStarInfos)
            //{
            //    m_objectives.Add(starInfo.objective);

            //    List<int> requirements = new List<int>();

            //    foreach (int requirement in starInfo.requirements)
            //    {
            //        requirements.Add(requirement);
            //    }

            //    m_levelStars.Add(starInfo.starType, starInfo.requirements);
            //}

            //foreach (LevelStarType starType in Enum.GetValues(typeof(LevelStarType)))
            //{
            //    m_levelStarStats.Add(starType, new List<int>());
            //    m_levelStarStats[starType].Add(0);
            //    m_levelStarStats[starType].Add(0);

            //    m_levelStarsAchieved.Add(starType, false);
            //}

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
            m_gameController.OnGamePaused();
            player.playerController.OnGamePaused();
            for (int index = 0; index < moles.Length; ++index)
            {
                moles[index].moleController.OnGamePaused();
            }
        }

        private void OnGameResumed()
        {
            m_gameController.OnGameResumed();
            player.playerController.OnGameResumed();
            for (int index = 0; index < moles.Length; ++index)
            {
                moles[index].moleController.OnGameResumed();
            }
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
            //// continue to store the stats even if stars have been achieved
            //m_levelStarStats[LevelStarType.Score][0] = m_gameController.CurrentScore;
            //m_levelStarStats[LevelStarType.HitPercentage][0] = (int)m_gameController.CurrentPercentage;
            //m_levelStarStats[LevelStarType.HitPercentage][1] = m_gameController.CurrentWhackAttempts;
            //m_levelStarStats[LevelStarType.MolesWhacked][0] = m_gameController.CurrentMolesWhacked;

            //// all stars have been achieved so leave method
            //int starsAchievedCount = 0;
            //foreach (bool starsAchieved in m_levelStarsAchieved.Values)
            //{
            //    if(!starsAchieved)
            //    {
            //        break;
            //    }

            //    ++starsAchievedCount;
            //}

            //StarsAchievedCount = starsAchievedCount;
            //if(starsAchievedCount == m_levelStarsAchieved.Count)
            //{
            //    return;
            //}

            //foreach (LevelStarType starType in Enum.GetValues(typeof(LevelStarType)))
            //{
            //    bool starAchieved = false;
            //    if(!m_levelStars.ContainsKey(starType))
            //    {
            //        continue;
            //    }

            //    // star has been achieved so continue in loop
            //    if (m_levelStarsAchieved[starType])
            //    {
            //        continue;
            //    }

            //    starAchieved = m_levelManager.CheckStarRequirement(m_levelManager.SelectedLevelInfo.zoneId,
            //                                                       m_levelManager.SelectedLevelInfo.levelId,
            //                                                       starType,
            //                                                       m_levelStarStats[starType]);

            //    if (starAchieved)
            //    {
            //        m_levelStarsAchieved[starType] = starAchieved;
            //    }
            //}
        }

        private void GameIsOver()
        {
            DisplayGameResults = true;
        }

        #endregion
    }
}
