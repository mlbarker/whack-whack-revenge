//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Interfaces
{
    using Assets.Scripts.Level;

    public interface ILevel
    {
        #region Public Methods

        int GetStarRequirement();
        bool SetStarRequirement(int requirement);

        #endregion
    }
}
