//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace UnityVS.Striker.CSharp_Test.Level
{
    public interface ILevelZone
    {
        void AddLevel(LevelId levelId, ILevel level);
        bool ContainsLevel(LevelId levelId);
    }
}
