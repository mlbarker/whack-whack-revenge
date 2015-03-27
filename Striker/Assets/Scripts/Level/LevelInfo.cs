//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Level
{
    public struct LevelInfo
    {
        public LevelZoneId zoneId;
        public LevelId levelId;

        public int levelTimeInSeconds;
        public int starScoreRequirement;
        public int starWhackPercentRequirement;
        public int starMolesWhackedRequirement;
    }
}
