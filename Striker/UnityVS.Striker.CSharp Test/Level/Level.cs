//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace UnityVS.Striker.CSharp_Test.Level
{
    using System.Collections.Generic;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Level;

    public class Level : ILevel
    {
        #region Private Members

        private Dictionary<LevelStarId, int> m_stars = new Dictionary<LevelStarId, int>();

        #endregion

        #region ILevel Methods

        public int GetStarRequirements(LevelStarId starId)
        {
            if(m_stars.ContainsKey(starId))
            {
                return m_stars[starId];
            }

            return -1;
        }

        public bool SetStarRequirements(LevelStarId starId, int requirement)
        {
            if(m_stars == null)
            {
                m_stars = new Dictionary<LevelStarId, int>();
            }

            if(m_stars.ContainsKey(starId))
            {
                return false;
            }

            m_stars.Add(starId, requirement);
            return true;
        }

        #endregion

        #region ICloneable Methods

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

    }
}
