//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Utilities.Timers
{
    public interface ITimer
    {
        bool Active();
        void SetTimer(int timeInSeconds);
        void StartTimer();
        void StopTimer();
        void ResetTimer();
        void AutoResetTimer(bool autoReset);
        void Update();
    }
}
