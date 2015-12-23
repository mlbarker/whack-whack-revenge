//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Formation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            //InitializePositions();
        }

        void Update()
        {
            UpdateFormation();
        }

        #endregion

        #region IFormation Methods

        public override void InitializePositions()
        {
            base.InitializePositions();
            RemoveExtraLocations();
            InitializeFormationController();
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods

        private void RemoveExtraLocations()
        {
            // Check to see if this is more than the maximum
            // amount of positions for this formation
            if (m_positionedMoles.Count > MAX_SIZE)
            {
                Location[] locations = m_positionedMoles.Keys.ToArray();
                for (int index = MAX_SIZE; index < locations.Length; ++index)
                {
                    m_positionedMoles.Remove(locations[index]);
                }
            }
        }

        #endregion
    }
}
