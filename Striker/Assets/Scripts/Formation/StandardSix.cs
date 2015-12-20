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

    public class StandardSix : BaseFormation
    {
        #region Constants

        private const int MAX_SIZE = 6;

        #endregion

        #region Fields

        #endregion

        #region Public Properties
        #endregion

        #region Editor Values

        #endregion

        #region Unity Methods

        void Start()
        {
            InitializePositions();
        }

        #endregion

        #region IFormation Methods

        public override void InitializePositions()
        {
            if(positionMoles == null)
            {
                throw new NullReferenceException("Moles and positions not set in editor");
            }

            // initialize the dictionary of moles with positions and empty lists
            m_positionedMoles = new Dictionary<int, List<Mole>>(MAX_SIZE);
            for (int index = 0; index < MAX_SIZE; ++index)
            {
                m_positionedMoles.Add(index, new List<Mole>());
            }

            // store moles for each position
            foreach (PositionMoles positionMole in positionMoles)
            {
                if(positionMole.position < MAX_SIZE)
                {
                    m_positionedMoles[positionMole.position].Add(positionMole.mole);
                }
            }
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion
    }
}
