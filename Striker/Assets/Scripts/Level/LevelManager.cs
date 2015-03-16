//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Level
{
    using System.Collections.Generic;
    using Assets.Scripts.Interfaces;

    public class LevelManager : ILevelManager
    {
        #region Private Members

        private static LevelManager m_instance;
        private Dictionary<LevelZoneId, ILevelZone> m_levelZones;

        #endregion

        #region Constructors

        private LevelManager()
        {
            m_levelZones = new Dictionary<LevelZoneId, ILevelZone>();
        }

        #endregion

        #region Public Properties

        public static LevelManager Instance
        {
            get
            {
                if(m_instance == null)
                {
                    m_instance = new LevelManager();
                }

                return m_instance;
            }
        }

        #endregion

        #region ILevelManager Methods

        public void AddZone(ILevelZone levelZone, LevelZoneId levelZoneId)
        {
            if(m_levelZones.ContainsKey(levelZoneId))
            {
                return;
            }

            m_levelZones.Add(levelZoneId, levelZone);
        }

        public bool ContainsZone(LevelZoneId levelZoneId)
        {
            return m_levelZones.ContainsKey(levelZoneId);
        }

        public void AddLevelToZone(LevelZoneId levelZoneId, LevelId levelId, ILevel level)
        {
            if(m_levelZones.ContainsKey(levelZoneId))
            {
                m_levelZones[levelZoneId].AddLevel(levelId, level);
            }
        }

        public bool ContainsLevel(LevelZoneId levelZoneId, LevelId levelId)
        {
            if(m_levelZones.ContainsKey(levelZoneId))
            {
                return m_levelZones[levelZoneId].ContainsLevel(levelId);
            }

            return false;
        }

        public int GetStarRequirements(LevelZoneId levelZoneId, LevelId levelId, LevelStarId starId)
        {
            if(!ContainsLevel(levelZoneId, levelId))
            {
                return -1;
            }

            return m_levelZones[levelZoneId].GetStarRequirements(levelId, starId);
        }

        public bool CheckStarRequirement(LevelZoneId levelZoneId, LevelId levelId, LevelStarId starId, int playerResult)
        {
            if(!ContainsLevel(levelZoneId, levelId))
            {
                return false;
            }

            return playerResult >= m_levelZones[levelZoneId].GetStarRequirements(levelId, starId);
        }

        #endregion

        #region Public Methods

        public void Clear()
        {
            // TODO: cannot keep this...
            m_levelZones.Clear();
        }

        #endregion
    }
}
