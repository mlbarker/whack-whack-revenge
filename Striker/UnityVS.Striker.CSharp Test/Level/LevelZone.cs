//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace UnityVS.Striker.CSharp_Test.Level
{
    using System;
    using System.Collections.Generic;

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

        public bool ContainsLevel(LevelId levelId)
        {
            return m_levels.ContainsKey(levelId);
        }

        #endregion
    }
}
