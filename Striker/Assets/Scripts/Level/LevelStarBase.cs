//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Level
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Assets.Scripts.Interfaces;

    [Serializable]
    public abstract class LevelStarBase : MonoBehaviour
    {
        #region Public Properties

        public int Requirement
        {
            get;
            protected set;
        }

        public string Objective
        {
            get;
            protected set;
        }

        public LevelStarType StarType
        {
            get;
            protected set;
        }

        public bool Achieved
        {
            get;
            private set;
        }

        #endregion

        #region Editor Values

        public int requirement;
        public string objective;
        public LevelStarType starType;

        #endregion

        #region LevelStarBase Methods

        protected abstract bool RequirementAchieved(List<int> currentAmounts);

        #endregion

        #region Public Methods

        public void Initialize()
        {
            Requirement = requirement;
            Objective = objective;
            StarType = starType;
        }

        public void UpdateStarStatus(List<int> currentAmounts)
        {
            if(RequirementAchieved(currentAmounts))
            {
                Achieved = true;
            }
        }

        #endregion
    }
}
