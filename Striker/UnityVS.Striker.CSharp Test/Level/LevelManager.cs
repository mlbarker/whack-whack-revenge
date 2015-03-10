//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace UnityVS.Striker.CSharp_Test.Level
{
    using System.Collections.Generic;

    public class LevelManager : ILevelManager
    {
        #region Private Methods

        private Dictionary<LevelZoneId, ILevelZone> m_levelZones = new Dictionary<LevelZoneId,ILevelZone>();

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

        #endregion
    }
}
