//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Interfaces
{
    public interface IGameTimeController
    {
        int GameTimeSeconds { get; }
        int GameTimeSecondsLeft { get; }

        void SetGameTime(int seconds);
        void UpdateTime();
        void TimeUpCallback();
    }
}
