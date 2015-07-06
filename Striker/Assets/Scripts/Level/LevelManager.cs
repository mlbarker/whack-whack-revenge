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
        private LevelInfo m_selectedLevelInfo;

        #endregion

        #region Constructors

        private LevelManager()
        {
            m_levelZones = new Dictionary<LevelZoneId, ILevelZone>();
            m_selectedLevelInfo = new LevelInfo();
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

        public LevelInfo SelectedLevelInfo
        { 
            get
            {
                return m_selectedLevelInfo;
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

        public LevelStarInfo GetStarRequirements(LevelZoneId levelZoneId, LevelId levelId, LevelStarType starType)
        {
            if(!ContainsLevel(levelZoneId, levelId))
            {
                return new LevelStarInfo();
            }

            return m_levelZones[levelZoneId].GetStarRequirements(levelId, starType);
        }

        public bool CheckStarRequirement(LevelZoneId levelZoneId, LevelId levelId, LevelStarType starType, List<int> playerResults)
        {
            if(!ContainsLevel(levelZoneId, levelId))
            {
                return false;
            }

            return m_levelZones[levelZoneId].CheckStarRequirement(levelId, starType, playerResults);
        }

        #endregion

        #region Public Methods

        public void StoreSelectedLevelInfo(LevelZoneId zoneId, LevelId levelId)
        {
            Level level = m_levelZones[zoneId].GetLevel(levelId) as Level;

            m_selectedLevelInfo.levelId = levelId;
            m_selectedLevelInfo.zoneId = zoneId;
            m_selectedLevelInfo.levelTimeInSeconds = level.LevelTimeInSeconds;

            m_selectedLevelInfo.levelStarInfos = level.GetStarInfos();
        }

        public LevelInfo GetLevelInfo(LevelZoneId zoneId, LevelId levelId)
        {
            Level level = m_levelZones[zoneId].GetLevel(levelId) as Level;
            LevelInfo levelInfo;

            levelInfo.levelId = levelId;
            levelInfo.zoneId = zoneId;
            levelInfo.levelTimeInSeconds = level.LevelTimeInSeconds;

            levelInfo.levelStarInfos = level.GetStarInfos();
            return levelInfo;
        }

        public void Clear()
        {
            foreach(LevelZoneId zoneId in m_levelZones.Keys)
            {
               m_levelZones[zoneId].Clear();
            }
        }

        #endregion
    }
}
