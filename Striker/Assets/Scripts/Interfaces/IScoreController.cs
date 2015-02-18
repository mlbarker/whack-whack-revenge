//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Interfaces
{
    public interface IScoreController
    {
        //void ScoreUpdate();
        //void WhackPercentageUpdate();

        int Score
        {
            get;
        }

        float WhackPercentage
        {
            get;
        }
    }
}
