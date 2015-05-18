//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Stars
{
    using System.Collections.Generic;
    using UnityEngine;
    using Assets.Scripts.Level;

    public class HitPercentStar : LevelStarBase
    {
        #region Constants

        private const int PERCENT_INDEX = 0;
        private const int ATTEMPTS_INDEX = 1;

        #endregion

        #region Editor Values

        public int whackAttemptsRequired;

        #endregion

        #region Protected Methods

        protected override bool RequirementAchieved(List<int> currentAmounts)
        {
            if (currentAmounts[PERCENT_INDEX] >= Requirement && currentAmounts[ATTEMPTS_INDEX] >= whackAttemptsRequired)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
