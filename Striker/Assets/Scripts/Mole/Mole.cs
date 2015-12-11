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
            moleAnimator.SetBool("Swoon", moleController.Swoon);
            moleAnimator.SetBool("IsUp", moleController.IsUp);
            moleAnimator.SetBool("Idle", moleController.Idle);
            Debug.Log("MoveIntoHole|IsMoving - " + moleController.IsMoving);
        }

        public void MoveOutOfHole()
        {
            moleAnimator.SetBool("Swoon", moleController.Swoon);
            moleAnimator.SetBool("IsUp", moleController.IsUp);
            moleAnimator.SetBool("Idle", moleController.Idle);
            Debug.Log("MoveOutOfHole|IsMoving - " + moleController.IsMoving);
        }

        public void MoveIntoHoleOnSwoon()
        {
            moleAnimator.SetBool("Swoon", moleController.Swoon);
            moleAnimator.SetBool("IsUp", moleController.IsUp);
            moleAnimator.SetBool("Idle", moleController.Idle);

            // TODO: need something better than this to transition to down
            moleAnimator.SetBool("SwoonToDown", false);
            Debug.Log("MoveIntoHoleOnSwoon|IsMoving - " + moleController.IsMoving);
        }

        public void MoveToIdle()
        {
            moleAnimator.SetBool("IsUp", moleController.IsUp);
            moleAnimator.SetBool("Idle", moleController.Idle);
        }

        public void MoveOnInjured()
        {
            moleAnimator.SetBool("IsUp", moleController.IsUp);
            moleAnimator.SetBool("Hit", moleController.Hit);
            moleAnimator.SetBool("Swoon", moleController.Swoon);
            moleAnimator.SetBool("Idle", moleController.Idle);
        }

        #endregion

        #region IHealthController Methods

        public void AdjustHealth()
        {
            moleController.DecrementHealth(1);
        }

        public void RecoverHealth()
        {
            moleController.IncrementHealth(1);
        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            moleController.SetMovementController(this);
            moleController.SetHealthController(this);

            moleAnimator = GetComponent<Animator>();
            if (moleAnimator == null)
            {
                throw new UnassignedReferenceException();
            }

            //moleAnimator.Play("MoveDown", 0, 6.0f);
        }

        public void StartMole()
        {
            moleAnimator.Play("MoveDown", 0, 6.0f);
            moleController.StartMole();
        }

        public void OnUpAnimationFinished()
        {
            moleController.StoppedMoving();
            Debug.Log("OnUpFinished|IsMoving - " + moleController.IsMoving);
        }

        public void OnDownAnimationFinished()
        {
            moleController.StoppedMoving();
            Debug.Log("OnDownFinished|IsMoving - " + moleController.IsMoving);
        }

        public void OnInjuredAnimationFinished()
        {
            moleController.TransitionInjuredToIdle();
        }

        public void OnSwoonAnimationFinished()
        {
            moleController.StoppedMoving();
            // TODO: need something better than this to transition to down
            moleAnimator.SetBool("SwoonToDown", true);
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