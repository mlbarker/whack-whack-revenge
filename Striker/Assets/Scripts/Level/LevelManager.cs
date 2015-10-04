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

        public bool GoToNextLevel 
        {
            get; 
            private set;
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
                return null;
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
            if(m_selectedLevelInfo != null)
            {
                m_selectedLevelInfo.Dispose();
            }

            m_selectedLevelInfo = new LevelInfo(
                Persistence.PersistentManager.Instance.GetLevelInfoData((int)zoneId, (int)levelId).ZoneId,
                Persistence.PersistentManager.Instance.GetLevelInfoData((int)zoneId, (int)levelId).LevelIdNum,
                Persistence.PersistentManager.Instance.GetLevelInfoData((int)zoneId, (int)levelId).LevelStarInfos,
                Persistence.PersistentManager.Instance.GetLevelInfoData((int)zoneId, (int)levelId).LevelTimeInSeconds);
        }

        public LevelInfo GetLevelInfo(LevelZoneId zoneId, LevelId levelId)
        {
            Level level = m_levelZones[zoneId].GetLevel(levelId) as Level;
            LevelInfo levelInfo = new LevelInfo(zoneId, levelId, level.GetStarInfos(), level.LevelTimeInSeconds);

            return levelInfo;
        }

        public int GetNextLevelSelected()
        {
            if(m_selectedLevelInfo == null)
            {
                // need to throw an exception here or log statement
                GoToNextLevel = false;
                return SceneIndices.LevelSelectScene;
            }

            // this convoluted mess is all because the level
            // scenes in Unity start at 2. therefore, last level
            // in a zone ends with 1 
            // (e.g. zone 1 -> 2-11, zone 2 -> 12-21)
            int levelOffset = 1;
            LevelZoneId currentZoneId = m_selectedLevelInfo.ZoneId;
            int lastLevelInZone = (int)(currentZoneId) + levelOffset;
            lastLevelInZone *= LevelZone.MAX_LEVELS + levelOffset;

            LevelId currentLevelId = m_selectedLevelInfo.LevelIdNum;
            if((int)currentLevelId == lastLevelInZone)
            {
                return SceneIndices.LevelSelectScene;
            }

            // to the next level
            currentLevelId++;
            StoreSelectedLevelInfo(currentZoneId, currentLevelId);
            return (int)currentLevelId;
        }

        public void NextLevelLoaded()
        {
            GoToNextLevel = false;
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
