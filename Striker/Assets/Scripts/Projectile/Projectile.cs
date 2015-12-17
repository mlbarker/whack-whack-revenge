//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Projectile
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Assets.Scripts.Utilities.Timers;

    public class Projectile : MonoBehaviour
    {
        #region Fields

        private float m_defaultAnimationTime;
        private Timer m_destroyTimer;
        private int m_timeTillDestroy;
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

        public int MaxHealth
        {
            get;
            private set;
        }

        public int Health
        {
            get;
            private set;
        }

        public bool Swoon
        {
            get
            {
                return Health < 1;
            }
        }

        public bool Hit
        {
            get;
            private set;
        }

        public bool ReadyForDestroy
        {
            get;
            private set;
        }

        public bool DefaultSwoonAnimation
        {
            get;
            private set;
        }

        #endregion

        #region Editor Values

        public float travelTime;
        public int maxHealth;

        #endregion

        #region Unity Methods

        void Start()
        {
            Initialize();
        }

        void Update()
        {
            UpdateDefaultSwoonAnimation();
            UpdateScale();
            ClearHit();
        }

        #endregion

        #region Public Methods

        public virtual void ProjectileDefeatedAnimation()
        {
            // animator controller calls here
        }

        public virtual void DecrementHealth()
        {
            if (!Swoon)
            {
                --Health;
                Hit = true;
            }
        }

        public void DestroyProjectile()
        {
            ReadyForDestroy = true;
        }

        #endregion

        #region Protected Methods

        protected virtual void Initialize()
        {
            TravelTime = travelTime;
            TravelTimeElapsed = false;
            m_startTime = Time.time;
            m_defaultAnimationTime = Time.time;

            Hit = false;

            MaxHealth = maxHealth;
            Health = maxHealth;

            ReadyForDestroy = false;
            m_timeTillDestroy = 2;
            m_destroyTimer = new Timer(m_timeTillDestroy, DestroyProjectile);
            m_destroyTimer.StopTimer();

            DefaultSwoonAnimation = true;
        }

        #endregion

        #region Private Methods

        private void UpdateScale()
        {
            if (!Swoon)
            {
                float scale = (Time.time - m_startTime) / TravelTime;
                if (scale >= 1.0f)
                {
                    TravelTimeElapsed = true;
                }

                transform.localScale += new Vector3(scale, scale);
                transform.Rotate(Vector3.forward * Time.time);
            }
        }

        private void UpdateDefaultSwoonAnimation()
        {
            if (Swoon && DefaultSwoonAnimation)
            {
                float translate = (Time.time - m_defaultAnimationTime) / TravelTime;
                transform.localPosition += (Vector3.up * -translate);
                if(!m_destroyTimer.Active())
                {
                    m_destroyTimer.SetTimer(m_timeTillDestroy);
                    m_destroyTimer.StartTimer();
                }

                // TODO: this is a HACK until I can figure out what's wrong with the timer.
                if(transform.localPosition.y < -100.0f)
                {
                    DestroyProjectile();
                }
            }
        }

        private void ClearHit()
        {
            Hit = false;
        }

        #endregion
    }
}
