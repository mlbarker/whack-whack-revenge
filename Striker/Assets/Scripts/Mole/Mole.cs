//-----------------------------
// ImperfectlyCoded © 2014
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
            // do visual logic here...
            //GoIntoHole = true;
            Debug.Log("In Hole");
            moleController.RestoreHealth();
            moleController.ToggleUp();
        }

        public void MoveOutOfHole()
        {
            // do visual logic here...
            //GoIntoHole = false;
            Debug.Log("Out Hole");
            moleController.ToggleUp();
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
        }

        #endregion

        #region Private Methods

        private void UpdateMole()
        {
            moleController.UpdateStatus();

            moleAnimator.SetBool("IsUp", moleController.IsUp);
        }

        #endregion
    }
}