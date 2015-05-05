//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Level
{
    using Assets.Scripts.Interfaces;

    public struct LevelInfo
    {
        public LevelZoneId zoneId;
        public LevelId levelId;
        public LevelStarInfo[] levelStarInfos;

        public int levelTimeInSeconds;
        //public int starScoreRequirement;
        //public int starWhackPercentRequirement;
        //public int starMolesWhackedRequirement;
    }
}
