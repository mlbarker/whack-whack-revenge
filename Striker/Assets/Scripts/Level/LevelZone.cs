﻿//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Level
{
    using System;
    using System.Collections.Generic;
    using Assets.Scripts.Interfaces;

    [Serializable]
    public class LevelZone : ILevelZone
    {
        #region Private Members

        private Dictionary<LevelId, ILevel> m_levels = new Dictionary<LevelId, ILevel>();

        #endregion

        #region ILevelZone Methods

        public void AddLevel(LevelId levelId, ILevel level)
        {
            if (m_levels.ContainsKey(levelId))
            {
                return;
            }

            m_levels.Add(levelId, level);
        }

        public ILevel GetLevel(LevelId levelId)
        {
            if (!m_levels.ContainsKey(levelId))
            {
                return null;
            }

            return m_levels[levelId];
        }

        public bool ContainsLevel(LevelId levelId)
        {
            return m_levels.ContainsKey(levelId);
        }

        public bool CheckStarRequirement(LevelId levelId, LevelStarType starType, List<int> playerResults)
        {
            if(!ContainsLevel(levelId))
            {
                return false;
            }

            m_levels[levelId].UpdateStarAchievement(starType, playerResults);
            bool achieved = m_levels[levelId].GetStarInfo(starType).requirementAchieved;
            return achieved;
        }

        public LevelStarInfo GetStarRequirements(LevelId levelId, LevelStarType starType)
        {
            if(!ContainsLevel(levelId))
            {
                return new LevelStarInfo();
            }

            return m_levels[levelId].GetStarInfo(starType);
        }

        #endregion
    }
}