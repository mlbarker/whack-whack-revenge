//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Projectile
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Projectile : MonoBehaviour
    {
        #region Fields

        private float m_scale;
        private float m_startTime;

        #endregion

        #region Public Properties

        public float TravelTime
        {
            get;
            private set;
        }

        public bool TravelTimeElapsed
        {
            get;
            private set;
        }

        #endregion

        #region Editor Values

        public float travelTime;

        #endregion

        #region Unity Methods

        void Start()
        {
            Initialize();
        }

        void Update()
        {
            UpdateScale();
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        private void Initialize()
        {
            TravelTime = travelTime;
            TravelTimeElapsed = false;
            m_startTime = Time.time;
        }

        private void UpdateScale()
        {
            float scale = (Time.time - m_startTime) / TravelTime;
            if(scale >= 1.0f)
            {
                TravelTimeElapsed = true;
            }

            transform.localScale += new Vector3(scale, scale);
            transform.Rotate(Vector3.forward * Time.time);
        }

        #endregion
    }
}
