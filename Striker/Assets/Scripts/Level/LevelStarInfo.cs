//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Level
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class LevelStarInfo
    {
        public string Objective
        {
            get;
            private set;
        }

        public List<int> Requirements
        {
            get;
            private set;
        }

        public bool RequirementAchieved
        {
            get;
            private set;
        }

        public LevelStarType StarType
        {
            get;
            private set;
        }

        private LevelStarInfo()
        {
        }

        public LevelStarInfo(string objective, List<int> requirements, bool requirementAchieved, LevelStarType starType)
        {
            Objective = objective;
            Requirements = requirements;
            RequirementAchieved = requirementAchieved;
            StarType = starType;
        }
    }
}
