//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Level
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    public class Zone : MonoBehaviour
    {
        #region Public Properties

        public int RequiredStars
        {
            get;
            private set;
        }

        #endregion

        #region Editor Values

        public int requiredStars;

        #endregion

        #region Unity Methods

        void Start()
        {
            RequiredStars = requiredStars;
        }

        #endregion
    }
}
