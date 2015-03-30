//-----------------------------
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

        public int GetStarRequirements(LevelId levelId, LevelStarId starId)
        {
            if(!ContainsLevel(levelId))
            {
                return -1;
            }

            return m_levels[levelId].GetStarRequirement(starId);
        }

        #endregion
    }
}
