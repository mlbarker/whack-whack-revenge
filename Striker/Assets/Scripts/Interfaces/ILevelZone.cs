//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Interfaces
{
    using System.Collections.Generic;
    using Assets.Scripts.Level;

    public interface ILevelZone
    {
        void AddLevel(LevelId levelId, ILevel level);
        ILevel GetLevel(LevelId levelId);
        bool ContainsLevel(LevelId levelId);
        bool CheckStarRequirement(LevelId levelId, LevelStarType starType, List<int> playerResults);
        LevelStarInfo GetStarRequirements(LevelId levelId, LevelStarType starType);
    }
}
