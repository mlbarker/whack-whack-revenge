//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Level
{
    using System;
    using System.Collections.Generic;
    using Assets.Scripts.Game;
    using Assets.Scripts.Interfaces;

    public class StarController : IPauseController
    {
        #region Fields

        private LevelManager m_levelManager;
        private GameController m_gameController;
        private Dictionary<LevelStarType, List<int>> m_levelStars = new Dictionary<LevelStarType, List<int>>();
        private Dictionary<LevelStarType, List<int>> m_levelStarStats = new Dictionary<LevelStarType, List<int>>();
        private Dictionary<LevelStarType, bool> m_levelStarsAchieved = new Dictionary<LevelStarType, bool>();
        private List<string> m_objectives = new List<string>();

        #endregion

        #region Public Properties

        public List<string> Objectives
        {
            get
            {
                return m_objectives;
            }
        }

        public int StarsAchievedCount
        {
            get;
            private set;
        }

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
            IsPaused = true;
        }

        public void OnGameResumed()
        {
            IsPaused = false;
        }

        #endregion

        #region Constructors

        public StarController(GameController gameController)
        {
            m_gameController = gameController;
        }

        private StarController()
        {
        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            InitializeObjectives();
            InitializeStats();
            AddToPauseManager();
        }

        public void UpdateStarsAchievements()
        {
            StoreStatsForStars();
            CheckStarAchievement();
            UpdateStarsAchieved();
        }

        public int GetStarStat(LevelStarType starType)
        {
            if(starType != LevelStarType.HitPercentage)
            {
                return m_levelStarStats[starType][0];
            }

            // must have certain amount of attempts
            if (m_levelStarStats[starType][1] >= m_levelStars[starType][1])
            {
                return m_levelStarStats[starType][0];
            }

            return 0;
        }
        

        #endregion

        #region Private Methods

        private void InitializeObjectives()
        {
            m_levelManager = LevelManager.Instance;
            LevelInfo levelInfo = m_levelManager.SelectedLevelInfo;
            foreach (LevelStarInfo starInfo in levelInfo.levelStarInfos)
            {
                m_objectives.Add(starInfo.objective);

                List<int> requirements = new List<int>();
                foreach (int requirement in starInfo.requirements)
                {
                    requirements.Add(requirement);
                }

                m_levelStars.Add(starInfo.starType, starInfo.requirements);
            }
        }

        private void InitializeStats()
        {
            foreach (LevelStarType starType in Enum.GetValues(typeof(LevelStarType)))
            {
                m_levelStarStats.Add(starType, new List<int>());
                m_levelStarStats[starType].Add(0);
                m_levelStarStats[starType].Add(0);

                m_levelStarsAchieved.Add(starType, false);
            }
        }

        private void AddToPauseManager()
        {
            PauseManager.Instance.Add(GetHashCode(), this);
        }

        private void StoreStatsForStars()
        {
            // continue to store the stats even if stars have been achieved
            m_levelStarStats[LevelStarType.Score][0] = m_gameController.CurrentScore;
            m_levelStarStats[LevelStarType.HitPercentage][0] = (int)m_gameController.CurrentPercentage;
            m_levelStarStats[LevelStarType.HitPercentage][1] = m_gameController.CurrentWhackAttempts;
            m_levelStarStats[LevelStarType.MolesWhacked][0] = m_gameController.CurrentMolesWhacked;
        }

        private bool UpdateStarsAchieved()
        {
            // all stars have been achieved so leave method
            int starsAchievedCount = 0;
            foreach (bool starsAchieved in m_levelStarsAchieved.Values)
            {
                if (starsAchieved)
                {
                    ++starsAchievedCount;
                    continue;
                }

                if(starsAchievedCount == 0)
                {
                    continue;
                }
            }

            StarsAchievedCount = starsAchievedCount;
            if (starsAchievedCount == m_levelStarsAchieved.Count)
            {
                return true;
            }

            return false;
        }

        private void CheckStarAchievement()
        {
            foreach (LevelStarType starType in Enum.GetValues(typeof(LevelStarType)))
            {
                bool starAchieved = false;
                if (!m_levelStars.ContainsKey(starType))
                {
                    continue;
                }

                starAchieved = m_levelManager.CheckStarRequirement(m_levelManager.SelectedLevelInfo.zoneId,
                                                                   m_levelManager.SelectedLevelInfo.levelId,
                                                                   starType,
                                                                   m_levelStarStats[starType]);

                m_levelStarsAchieved[starType] = starAchieved;
            }
        }

        #endregion
    }
}
