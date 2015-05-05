//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Stars
{
    using System.Collections.Generic;
    using UnityEngine;
    using Assets.Scripts.Level;

    public class ScoreStar : LevelStarBase
    {
        #region Constants

        private int SCORE_INDEX = 0;

        #endregion

        #region Protected Methods

        protected override bool RequirementAchieved(List<int> currentAmounts)
        {
            return currentAmounts[SCORE_INDEX] >= Requirement;
        }

        #endregion
    }
}
