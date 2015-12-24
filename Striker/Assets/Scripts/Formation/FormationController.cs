//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Formation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Assets.Scripts.Mole;
    using Assets.Scripts.Utilities.Timers;

    public class FormationController
    {
        #region Fields

        private Dictionary<Location, Mole> m_activeMoles;
        private Dictionary<Location, List<Mole>> m_positionedMoles;
        private Utilities.Random.Random m_random;
        private int m_inactiveLayer = 8;
        private int m_activeLayer = 0;

        #endregion

        #region Public Methods

        public void Initialize(Dictionary<Location, List<Mole>> positionedMoles)
        {
            m_activeMoles = new Dictionary<Location, Mole>();
            m_random = new Utilities.Random.Random();

            m_positionedMoles = positionedMoles;

            DeactivateAll();
            RandomizeActiveMoles();
        }

        public void UpdatePosition()
        {
            foreach (KeyValuePair<Location, Mole> activeMole in m_activeMoles.ToArray())
            {
                // check to see if out of hole... once in hole, set to inactive
                // and select random mole in that position for next active
                if (!activeMole.Value.ReadyForPositionChange)
                {
                    continue;
                }

                activeMole.Value.ClearPositionChangeFlag();
                activeMole.Value.StopMole();
                activeMole.Value.gameObject.layer = m_inactiveLayer;
                activeMole.Value.SetActive(false);
                Location location = activeMole.Key;
                RemoveMoleFromActiveList(location);
                SetRandomMoleToActiveList(location);
            }
        }

        #endregion

        #region Protected Methods

        protected void RandomizeActiveMoles()
        {
            if (m_random == null)
            {
                m_random = new Utilities.Random.Random();
            }

            m_activeMoles.Clear();

            foreach (KeyValuePair<Location, List<Mole>> positionedMole in m_positionedMoles)
            {
                int maxValue = positionedMole.Value.Count - 1;
                int randomIndex = m_random.RandomInt(maxValue);
                if (!m_activeMoles.ContainsKey(positionedMole.Key))
                {
                    positionedMole.Value[randomIndex].gameObject.layer = m_activeLayer;
                    positionedMole.Value[randomIndex].SetActive(true);
                    m_activeMoles.Add(positionedMole.Key, positionedMole.Value[randomIndex]);
                }
            }
        }

        protected void DeactivateAll()
        {
            foreach (KeyValuePair<Location, List<Mole>> positionedMole in m_positionedMoles)
            {
                positionedMole.Value.ForEach(x => x.gameObject.layer = m_inactiveLayer);
                positionedMole.Value.ForEach(x => x.SetActive(false));
            }
        }

        #endregion

        #region Private Methods

        private void RemoveMoleFromActiveList(Location location)
        {
            if(m_activeMoles.ContainsKey(location))
            {
                m_activeMoles.Remove(location);
            }
        }

        private void SetRandomMoleToActiveList(Location location)
        {
            int maxValue = m_positionedMoles[location].Count - 1;
            int randomIndex = m_random.RandomInt(maxValue);
            m_activeMoles[location] = m_positionedMoles[location][randomIndex];
            m_activeMoles[location].gameObject.layer = m_activeLayer;
            m_activeMoles[location].SetActive(true);
            m_activeMoles[location].StartMole();
        }

        #endregion
    }
}
