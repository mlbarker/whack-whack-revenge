//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Interfaces
{
    using Assets.Scripts.Level;

    public interface ILevelManager
    {
        void AddZone(ILevelZone levelZone, LevelZoneId levelZoneId);
        bool ContainsZone(LevelZoneId levelZoneId);
        void AddLevelToZone(LevelZoneId levelZoneId, LevelId levelId, ILevel level);
        bool ContainsLevel(LevelZoneId levelZoneId, LevelId levelId);
        int GetStarRequirements(LevelZoneId levelZoneId, LevelId levelId, LevelStarId starId);
        bool CheckStarRequirement(LevelZoneId levelZoneId, LevelId levelId, LevelStarId starId, int playerResult);
    }
}
