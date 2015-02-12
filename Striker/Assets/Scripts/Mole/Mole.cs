//-----------------------------
// ImperfectlyCoded © 2014-2015
//-----------------------------

namespace Assets.Scripts.Mole
{
    using System.Collections;
    using UnityEngine;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Utilities.Timers;

    public class Mole : MonoBehaviour, IMovementController, IHealthController
    {
        #region Private Members

        private Animator moleAnimator;

        #endregion

        #region Editor Values

        public MoleController moleController;

        #endregion

        #region Unity Methods

        void Start()
        {
            
        }

        void Update()
        {
            UpdateMole();
        }

        #endregion

        #region IMovementController Methods

        public void MoveIntoHole()
        {
            moleAnimator.SetBool("IsUp", moleController.IsUp);
        }

        public void MoveOutOfHole()
        {
            moleAnimator.SetBool("IsUp", moleController.IsUp);
        }

        #endregion

        #region IHealthController Methods

        public void AdjustHealth()
        {
            moleController.DecrementHealth(1);
        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            moleController.Initialize();

            moleController.SetMovementController(this);
            moleController.SetHealthController(this);

            moleAnimator = GetComponent<Animator>();
            if (moleAnimator == null)
            {
                throw new UnassignedReferenceException();
            }

            moleAnimator.Play("MoveDown", 0, 6.0f);
        }

        public void OnUpAnimationFinished()
        {
            moleController.StoppedMoving();
        }

        public void OnDownAnimationFinished()
        {
            moleController.StoppedMoving();
        }

        #endregion

        #region Private Methods

        private void UpdateMole()
        {
            moleController.Update();
        }

        #endregion
    }
}