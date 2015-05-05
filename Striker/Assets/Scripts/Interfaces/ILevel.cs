//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Interfaces
{
    using System.Collections.Generic;
    using Assets.Scripts.Level;

    public interface ILevel
    {
        #region Public Methods

        void SetStar(LevelStarBase levelStar);
        LevelStarInfo GetStarInfo(LevelStarType starType);
        void UpdateStarAchievement(LevelStarType starType, List<int> requirements);

        #endregion
    }
}
