//-----------------------------
// ImperfectlyCoded © 2014-2015
//-----------------------------

namespace Assets.Scripts.Mole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Assets.Scripts.Interfaces;

    public class PitcherMole : Mole, IAttackController
    {
        #region IAttackController Methods

        public void Attack()
        {
            moleAnimator.SetBool("Attack", moleController.Attack);
        }

        #endregion

        #region Animation Event Methods

        public void OnAttackMidway()
        {
            // create baseball and update somehow
        }

        public void OnAttackAnimationFinished()
        {
            moleController.StoppedMoving();
            moleController.ClearAttack();
            moleAnimator.SetBool("Attack", moleController.Attack);
        }

        #endregion

        #region Public Methods

        public override void Initialize()
        {
            // Initialize from base Mole
            base.Initialize();
            moleController.SetAttackController(this);
        }

        #endregion
    }
}
