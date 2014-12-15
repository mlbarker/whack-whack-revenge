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

        private Vector2 fromPosition;
        private Vector2 toPosition;

        #endregion

        #region Public Properties

        public bool GoIntoHole
        {
            get;
            private set;
        }

        #endregion

        #region Editor Values

        public MoleController moleController;

        #endregion

        #region Unity Methods

        void Start()
        {
            moleController.Initialize();

            moleController.SetMovementController(this);
            moleController.SetHealthController(this);

            fromPosition = new Vector2(0.0f, 2.0f);
            toPosition = new Vector2(0.0f, -2.0f);

            transform.position = toPosition;
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
            GoIntoHole = true;
        }

        public void MoveOutOfHole()
        {
            // do visual logic here...
            GoIntoHole = false;
        }

        #endregion

        #region IHealthController Methods

        public void AdjustHealth()
        {
            moleController.DecrementHealth(1);
        }

        #endregion

        #region Private Methods

        private void UpdateMole()
        {
            moleController.UpdateStatus();

            if (GoIntoHole)
            {
                transform.position = Vector2.Lerp(fromPosition, toPosition, 1.0f);
            }
            else
            {
                transform.position = Vector2.Lerp(toPosition, fromPosition, 1.0f);
            }
        }

        #endregion
    }
}