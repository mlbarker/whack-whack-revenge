//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Score
{
    using System;
    using Assets.Scripts.Interfaces;

    [Serializable]
    public class ScoreController : IScoreController
    {
        #region Private Members

        private int m_score;
        private float m_whackPercentage;

        #endregion

        #region Public Properties

        public int Score
        {
            get
            {
                return m_score;
            }
        }

        public float WhackPercentage
        {
            get
            {
                m_whackPercentage = (float)(Whacks) / (float)(WhackAttempts) * 100.0f;
                return float.IsNaN(m_whackPercentage) ? 0 : m_whackPercentage;
            }
        }

        public int MolesWhacked
        {
            get;
            private set;
        }

        public int Whacks
        {
            get;
            private set;
        }

        public int WhackAttempts
        {
            get;
            private set;
        }

        public bool StarScoreAchieved
        {
            get
            {
                return m_score >= scoreNeedForStar;
            }
        }

        public bool StarPercentageAchieved
        {
            get
            {
                return WhackPercentage >= whackPercentNeedForStar;
            }
        }

        public bool StarMolesWhackedAchieved
        {
            get
            {
                return MolesWhacked >= molesWhackedNeedForStar;
            }
        }

        #endregion

        #region Editor Values

        public int scoreNeedForStar;
        public float whackPercentNeedForStar;
        public int molesWhackedNeedForStar;

        #endregion

        #region Public Methods

        public void IncrementMolesWhacked()
        {
            ++MolesWhacked;
        }

        public void IncreaseScore(int points)
        {
            m_score += points;
        }

        public void RecordWhackAttempt(bool successfulAttempt)
        {
            if(successfulAttempt)
            {
                ++Whacks;
            }

            ++WhackAttempts;
        }

        #endregion
    }
}
