//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Interfaces
{
    public interface IPauseController
    {
        void OnGamePaused();
        void OnGameResumed();

        bool IsPaused { get; }
    }
}
