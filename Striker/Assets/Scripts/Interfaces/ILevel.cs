//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Interfaces
{
    using Assets.Scripts.Level;
    public interface ILevel
    {
        #region Public Methods

        int GetStarRequirements(LevelStarId starId);
        bool SetStarRequirements(LevelStarId starId, int requirement);

        #endregion
    }
}
