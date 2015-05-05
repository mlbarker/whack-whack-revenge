//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Level
{
    using System.Collections.Generic;

    public struct LevelStarInfo
    {
        public string objective;
        public List<int> requirements;
        public bool requirementAchieved;
        public LevelStarType starType;
    }
}
