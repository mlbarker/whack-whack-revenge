//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Player
{
    using System;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Utilities.Timers;

    [Serializable]
    public class PlayerController
    {
        #region Private Members

        private IInputController m_inputController;
        private Timer m_buttonDelayTimer;
        private int m_buttonDelaySeconds = 1;
        
        #endregion

        #region Public Properties

        public bool CanWhack
        {
            get;
            private set;
        }

        public bool WhackTriggered
        {
            get;
            private set;
        }

        public bool WhackCooldown
        {
            get
            {
                return m_buttonDelayTimer.Active();
            }
        }

        public bool MoleHit
        {
            get;
            private set;
        }

        #endregion

        #region Editor Values

        public int buttonDelayMilliseconds;

        #endregion

        #region Public Methods

        public void Initialize()
        {
            CanWhack = true;
            m_buttonDelayTimer = new Timer(m_buttonDelaySeconds, ButtonDelayTimeElapsed);
            m_buttonDelayTimer.AutoResetTimer(false);
        }

        public void Update()
        {
            UpdateInput();
            UpdateTimer();
        }

        public void SetInputController(IInputController inputController)
        {
            m_inputController = inputController;
            if(m_inputController == null)
            {
                throw new InputControllerException();
            }
        }

        #endregion

        #region Private Methods

        private void ButtonDelayTimeElapsed()
        {
            CanWhack = true;
        }

        private void UpdateInput()
        {
            WhackTriggered = m_inputController.AttackButton();
            if (WhackTriggered && !CanWhack)
            {
                return;
            }
            else if (WhackTriggered && CanWhack)
            {
                CanWhack = false;
                m_buttonDelayTimer.StartTimer();
            }
        }

        private void UpdateTimer()
        {
            m_buttonDelayTimer.Update();
        }

        #endregion
    }
}
