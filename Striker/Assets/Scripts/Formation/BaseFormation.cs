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

    public abstract class BaseFormation : MonoBehaviour, IFormation
    {
        #region Fields

        protected FormationController m_formationController;
        protected Dictionary<Location, List<Mole>> m_positionedMoles = new Dictionary<Location, List<Mole>>();
        protected List<Mole> m_moles = new List<Mole>();

        #endregion

        #region Public Properties

        public Mole[] Moles
        {
            get
            {
                return m_moles.ToArray();
            }
        }

        #endregion

        #region Editor Values

        #endregion

        #region Unity Methods

        void Start()
        {
        }

        #endregion

        #region IFormation Methods

        public virtual void InitializePositions()
        {
            GameObject[] positions = GameObject.FindGameObjectsWithTag("Position");
            if(positions == null)
            {
                throw new NullReferenceException("Position[] GameObject is null");
            }

            foreach (GameObject positionObject in positions)
            {
                Position position = positionObject.GetComponent<Position>();
                if(position == null)
                {
                    throw new NullReferenceException("Position Component is null");
                }

                Mole[] moles = positionObject.GetComponentsInChildren<Mole>();
                if(moles == null)
                {
                    throw new NullReferenceException("Mole[] Component is null");
                }

                // TODO: need to remove inactive moles
                m_moles.AddRange(moles);

                Location location = position.location;                
                m_positionedMoles.Add(location, new List<Mole>(moles));
            }
        }

        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods

        protected void InitializeFormationController()
        {
            m_formationController = new FormationController();
            m_formationController.Initialize(m_positionedMoles);
        }

        protected virtual void UpdateFormation()
        {
            m_formationController.UpdatePosition();
        }

        #endregion

        #region Private Methods
        #endregion
    }
}
