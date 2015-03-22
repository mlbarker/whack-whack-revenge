//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Interfaces
{
    using System;
    using Assets.Scripts.Level;

    public interface ILevel : ICloneable
    {
        #region Public Methods

        int GetStarRequirements(LevelStarId starId);
        bool SetStarRequirements(LevelStarId starId, int requirement);
        new object Clone();

        #endregion
    }
}
