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

        private List<ILevelStar> m_stars = new List<ILevelStar>(MAX_STARS);
        //private Dictionary<LevelStarId, int> m_stars = new Dictionary<LevelStarId, int>();

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
        public int levelTimeSeconds;
        //public int molesWhackedStar;
        //public int whackPercentStar;
        //public int scoreNeededStar;

        #endregion

        #region Unity Methods

        void Start()
        {
            Initialize();
        }

        #endregion

        #region ILevel Methods

        public int GetStarRequirements(LevelStarId starId)
        {
            if (m_stars.ContainsKey(starId))
            {
                return m_stars[starId];
            }

            return -1;
        }

        public bool SetStarRequirement(LevelStarId starId, int requirement)
        {
            if (m_stars == null)
            {
                m_stars = new Dictionary<LevelStarId, int>();
            }

            if (m_stars.ContainsKey(starId))
            {
                return false;
            }

            m_stars.Add(starId, requirement);
            return true;
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            Debug.Log(this.name);
            SetStarRequirement(LevelStarId.Score, scoreNeededStar);
            SetStarRequirement(LevelStarId.Hits, molesWhackedStar);
            SetStarRequirement(LevelStarId.HitPercent, whackPercentStar);

            LevelTimeInSeconds = levelTimeSeconds;
        }

        #endregion
    }
}
