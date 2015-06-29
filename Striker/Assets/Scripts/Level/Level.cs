//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Level
{
    using System.Collections.Generic;
    using UnityEngine;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Level;

    public class Level : MonoBehaviour, ILevel
    {
        #region Constants

        public const int MAX_STARS = 3;

        #endregion

        #region Private Members

        private List<LevelStarBase> m_stars = new List<LevelStarBase>(MAX_STARS);

        #endregion

        #region Public Properties

        public int LevelTimeInSeconds 
        { 
            get; 
            private set; 
        }

        #endregion

        #region Editor Values

        public LevelId levelId;
        public LevelZoneId zoneId;
        public LevelStarBase[] levelStars;
        public int levelTimeSeconds;

        #endregion

        #region Unity Methods

        void Start()
        {
            Initialize();
        }

        #endregion

        #region ILevel Methods

        public void SetStar(LevelStarBase levelStar)
        {
            if (m_stars.Count == MAX_STARS)
            {
                return;
            }

            if (levelStar == null)
            {
                return;
            }

            levelStar.Initialize();
            m_stars.Add(levelStar);
        }

        public LevelStarInfo GetStarInfo(LevelStarType starType)
        {
            foreach (LevelStarBase star in m_stars)
            {
                if(star.StarType == starType)
                {
                    LevelStarInfo starInfo = GetLevelStarInfo(star);
                    return starInfo;
                }
            }

            // filled with zeros...
            return new LevelStarInfo();
        }

        public void UpdateStarAchievement(LevelStarType starType, List<int> requirements)
        {
            foreach (LevelStarBase star in m_stars)
            {
                if(star.StarType == starType)
                {
                    star.UpdateStarStatus(requirements);
                    break;
                }
            }
        }

        #endregion

        #region Public Methods

        public LevelStarInfo[] GetStarInfos()
        {
            LevelStarInfo[] levelStarInfos = new LevelStarInfo[MAX_STARS];

            for (int index = 0; index < MAX_STARS; ++index)
            {
                LevelStarInfo starInfo = GetLevelStarInfo(m_stars[index]);
                levelStarInfos[index] = starInfo;
            }

            return levelStarInfos;
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            Debug.Log(this.name);

            foreach(LevelStarBase levelStar in levelStars)
            {
                SetStar(levelStar);
            }

            LevelTimeInSeconds = levelTimeSeconds;
        }

        private LevelStarInfo GetLevelStarInfo(LevelStarBase star)
        {
            LevelStarInfo starInfo = new LevelStarInfo();
            starInfo.objective = star.Objective;
            starInfo.requirementAchieved = star.Achieved;
            starInfo.starType = star.StarType;

            starInfo.requirements = new List<int>();
            starInfo.requirements.Add(star.Requirement);

            // TODO: the only star with two requirements... need to find a better way to do this
            Stars.HitPercentStar hitPercentStar = star as Stars.HitPercentStar;
            if(hitPercentStar != null)
            {
                starInfo.requirements.Add(hitPercentStar.whackAttemptsRequired);
            }

            return starInfo;
        }

        #endregion
    }
}
