﻿//-----------------------------
// ImperfectlyCoded © 2014-2016
//-----------------------------

namespace Assets.Scripts.Mole
{
    using System.Collections;
    using UnityEngine;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Utilities.Timers;

    public class Mole : MonoBehaviour, IMovementController, IHealthController, IEndGame
    {
        #region Protected Members

        protected Animator moleAnimator;

        #endregion

        #region Editor Values

        public MoleController moleController;

        #endregion

        #region Public Properties

        public bool ReadyForPositionChange
        {
            get
            {
                return moleController.CompletedCycle && !moleController.IsMoving;
            }
        }

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
        }

        public void MoveOutOfHole()
        {
            moleAnimator.SetBool("Swoon", moleController.Swoon);
            moleAnimator.SetBool("IsUp", moleController.IsUp);
            moleAnimator.SetBool("Idle", moleController.Idle);
        }

        public void MoveIntoHoleOnSwoon()
        {
            moleAnimator.SetBool("Swoon", moleController.Swoon);
            moleAnimator.SetBool("IsUp", moleController.IsUp);
            moleAnimator.SetBool("Idle", moleController.Idle);
            moleAnimator.SetBool("Attack", moleController.Attack);

            // TODO: need something better than this to transition to down
            moleAnimator.SetBool("SwoonToDown", false);
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

        #region IEndGame Methods

        public virtual void OnEndGame(bool playerDefeated)
        {
            StopMole();
            moleAnimator.SetBool("Celebrate", playerDefeated);
            moleAnimator.SetBool("Defeated", !playerDefeated);
        }

        #endregion

        #region Public Methods

        public virtual void Initialize()
        {
            moleController.SetMovementController(this);
            moleController.SetHealthController(this);

            moleAnimator = GetComponent<Animator>();
            if (moleAnimator == null)
            {
                throw new UnassignedReferenceException();
            }

            Game.EndGameManager.Instance.Add(GetHashCode(), this);
        }

        public void StartMole()
        {
            moleController.StartMole();
        }

        public void StopMole()
        {
            moleController.StopMole();
        }

        public void ClearPositionChangeFlag()
        {
            moleController.ClearCycle();
        }

        #endregion

        #region Animation Event Methods

        public void OnUpAnimationFinished()
        {
            moleController.StoppedMoving();
        }

        public void OnDownAnimationFinished()
        {
            moleController.StoppedMoving();
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

        public void SetActive(bool active)
        {
            moleController.SetActive(active);
            GetComponent<BoxCollider2D>().enabled = active;
            GetComponent<Renderer>().enabled = active;
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