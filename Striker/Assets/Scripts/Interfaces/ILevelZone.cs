//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Interfaces
{
    using Assets.Scripts.Level;

    public interface ILevelZone
    {
        void AddLevel(LevelId levelId, ILevel level);
        bool ContainsLevel(LevelId levelId);
        int GetStarRequirements(LevelId levelId, LevelStarId starId);
    }
}
