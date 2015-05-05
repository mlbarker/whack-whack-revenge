//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Stars
{
    using System.Collections.Generic;
    using UnityEngine;
    using Assets.Scripts.Level;

    public class MolesWhackedStar : LevelStarBase
    {
        #region Constants

        private int WHACKED_INDEX = 0;

        #endregion

        #region Protected Methods

        protected override bool RequirementAchieved(List<int> currentAmounts)
        {
            return currentAmounts[WHACKED_INDEX] >= Requirement;
        }

        #endregion
    }
}