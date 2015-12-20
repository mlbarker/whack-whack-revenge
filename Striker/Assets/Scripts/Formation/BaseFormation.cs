//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Formation
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Mole;

    public abstract class BaseFormation : MonoBehaviour, IFormation
    {
        #region Fields

        protected Dictionary<int, List<Mole>> m_positionedMoles;

        #endregion

        #region Public Properties
        #endregion

        #region Editor Values

        public PositionMoles[] positionMoles;

        #endregion

        #region Unity Methods

        void Start()
        {
            InitializePositions();
        }

        #endregion

        #region IFormation Methods

        public virtual void InitializePositions()
        {
            if (positionMoles == null)
            {
                throw new NullReferenceException("Moles and positions not set in editor");
            }

            //// initialize the dictionary of moles with positions and empty lists
            //for (int index = 0; index < MAX_SIZE; ++index)
            //{
            //    m_positionedMoles.Add(index, new List<Mole>());
            //}

            //// store moles for each position
            //foreach (PositionMoles positionMole in positionMoles)
            //{
            //    if (positionMole.position < MAX_SIZE)
            //    {
            //        m_positionedMoles[positionMole.position].Add(positionMole.mole);
            //    }
            //}
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion
    }
}
