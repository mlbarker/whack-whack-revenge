//-----------------------------
// ImperfectlyCoded © 2014-2015
//-----------------------------

namespace Assets.Scripts.Mole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Projectile;

    public class PitcherMole : Mole, IAttackController
    {
        #region Fields

        private GameObject m_clone;

        #endregion

        #region Editor Values

        public GameObject baseball;

        #endregion

        #region Unity Methods

        void Update()
        {
            if(m_clone == null)
            {
                return;
            }

            if(m_clone.GetComponent<Projectile>().TravelTimeElapsed || m_clone.GetComponent<Projectile>().ReadyForDestroy)
            {
                Destroy(m_clone);
                m_clone = null;
            }
        }

        #endregion

        #region IAttackController Methods

        public void Attack()
        {
            moleAnimator.SetBool("Attack", moleController.Attack);
        }

        #endregion

        #region Animation Event Methods

        public void OnAttackMidway()
        {
            // create baseball and update
            m_clone = (GameObject)Instantiate(baseball, transform.position, transform.rotation);
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
