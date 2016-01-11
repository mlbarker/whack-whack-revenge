//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Hud
{
    using System;
    using UnityEngine;

    public class HealthUI : MonoBehaviour
    {
        #region Public Properties

        public uint HealthIndex
        {
            get;
            private set;
        }

        #endregion

        #region Unity Methods

        void Start()
        {
            HealthIndex = healthIndex;
        }

        #endregion

        #region Editor Values

        public uint healthIndex;

        #endregion
    }
}
