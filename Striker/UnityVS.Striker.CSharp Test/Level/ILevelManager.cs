//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace UnityVS.Striker.CSharp_Test.Level
{
    public interface ILevelManager
    {
        void AddZone(ILevelZone levelZone, LevelZoneId levelZoneId);
        bool ContainsZone(LevelZoneId levelZoneId);
        void AddLevelToZone(LevelZoneId levelZoneId, LevelId levelId, ILevel level);
        bool ContainsLevel(LevelZoneId levelZoneId, LevelId levelId);
    }
}
