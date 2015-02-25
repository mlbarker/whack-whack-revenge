//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Score
{
    using Assets.Scripts.Interfaces;

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
                return m_whackPercentage;
            }
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

        #endregion

        #region Public Methods

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
