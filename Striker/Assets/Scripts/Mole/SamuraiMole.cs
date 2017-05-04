//-----------------------------
// ImperfectlyCoded © 2017
//-----------------------------

namespace Assets.Scripts.Mole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Assets.Scripts.Interfaces;

    public class SamuraiMole : Mole, ICounterController
    {
        #region Fields

        private int m_counterStanceSeconds;
        
        #endregion

        #region Editor Values

        public int counterStanceSeconds;

        #endregion

        #region ICounterController Methods

        public void CounterStance()
        {
            moleAnimator.SetBool("CounterStance", moleController.CounterStance);
        }

        public void CounterAttack()
        {
            moleAnimator.SetBool("CounterAttack", moleController.CounterAttack);
        }

        #endregion

        #region Public Methods

        public override void Initialize()
        {
            // Initialize from base Mole
            base.Initialize();
            SetMoleType(MoleType.SAMURAI_MOLE);
            m_counterStanceSeconds = counterStanceSeconds;
            moleController.SetCounterController(this);
        }

        #endregion
    }
}