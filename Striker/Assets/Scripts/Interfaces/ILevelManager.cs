//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Interfaces
{
    using System.Collections.Generic;
    using Assets.Scripts.Level;

    public interface ILevelManager
    {
        void AddZone(ILevelZone levelZone, LevelZoneId levelZoneId);
        bool ContainsZone(LevelZoneId levelZoneId);
        void AddLevelToZone(LevelZoneId levelZoneId, LevelId levelId, ILevel level);
        bool ContainsLevel(LevelZoneId levelZoneId, LevelId levelId);
        LevelStarInfo GetStarRequirements(LevelZoneId levelZoneId, LevelId levelId, LevelStarType starType);
        bool CheckStarRequirement(LevelZoneId levelZoneId, LevelId levelId, LevelStarType starType, List<int> playerResults);
    }
}
