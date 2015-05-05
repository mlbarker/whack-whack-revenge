//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Interfaces
{
    public interface ILevelStar
    {
        void SetObjective(string objective);
        void SetRequirement(int requirement);
        bool RequirementAchieved(int requirement);
    }
}
