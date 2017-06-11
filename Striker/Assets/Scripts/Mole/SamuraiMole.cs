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

        public void CounterStanceStart()
        {
            // CounterStance should be true in this method
            moleAnimator.SetBool("CounterStance", moleController.CounterStance);
            moleAnimator.SetBool("Idle", moleController.Idle);
        }

        public void CounterStanceEnd()
        {
            // CounterStance should be false in this method
            moleAnimator.SetBool("CounterStance", moleController.CounterStance);
            moleAnimator.SetBool("Idle", moleController.Idle);
        }

        public void CounterAttack()
        {
            moleAnimator.SetBool("CounterAttack", moleController.CounterAttack);
            moleAnimator.SetBool("CounterStance", moleController.CounterStance);
            moleAnimator.SetBool("Idle", moleController.Idle);
            moleAnimator.SetBool("Hit", moleController.Hit);
        }

        #endregion

        #region Animation Event Methods

        public void OnCounterStanceAnimationFinished()
        {
            moleController.StoppedMoving();
        }

        public void OnCounterStanceEndAnimationFinished()
        {
            moleController.StoppedMoving();
            moleController.ClearCounterStance();

            moleAnimator.SetBool("CounterStance", moleController.CounterStance);
            moleAnimator.SetBool("CounterAttack", moleController.CounterAttack);
            moleAnimator.SetBool("Idle", moleController.Idle);
        }

        public void OnCounterAttackAnimationFinished()
        {
            moleController.StoppedMoving();
            moleController.ClearCounterAttack();

            moleAnimator.SetBool("CounterStance", moleController.CounterStance);
            moleAnimator.SetBool("CounterAttack", moleController.CounterAttack);
            moleAnimator.SetBool("Idle", moleController.Idle);
            moleAnimator.SetBool("Hit", moleController.Hit);
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