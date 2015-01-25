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

        private IHitController m_hitController;
        private IInputController m_inputController;
        
        #endregion

        #region Public Properties

        public bool WhackTriggered
        {
            get;
            private set;
        }

        public bool MoleHit
        {
            get;
            private set;
        }

        #endregion

        #region Editor Values

        #endregion

        #region Public Methods

        public void Initialize()
        {
        }

        public void Update()
        {
            UpdateInput();
            UpdateHit();
        }

        public void SetInputController(IInputController inputController)
        {
            m_inputController = inputController;
            if(m_inputController == null)
            {
                throw new InputControllerException();
            }
        }

        public void SetHitController(IHitController hitController)
        {
            m_hitController = hitController;
            if(m_hitController == null)
            {
                throw new HitControllerException();
            }
        }

        #endregion

        #region Private Methods

        private void UpdateInput()
        {
            WhackTriggered = m_inputController.AttackButton();
        }

        private void UpdateHit()
        {
            if(!WhackTriggered)
            {
                MoleHit = false;
                return;
            }

            MoleHit = m_hitController.HitDetected();
        }

        #endregion
    }
}
