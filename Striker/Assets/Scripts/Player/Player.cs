﻿//-----------------------------
// ImperfectlyCoded © 2014-2015
//-----------------------------

namespace Assets.Scripts.Player
{
    using System;
    using UnityEngine;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Player;

    public class Player : MonoBehaviour, IInputController, IHitController
    {
        #region Public Properties

        public int HitCollisionId
        {
            get;
            private set;
        }

        public int WhackAttempts
        {
            get;
            private set;
        }

        public int Whacks
        {
            get;
            private set;
        }

        #endregion

        #region Private Properties

        private bool FingerCount
        {
            get;
            set;
        }

        #endregion

        #region Editor Values

        public PlayerController playerController;

        #endregion

        #region IInputController Methods

        public bool AttackButton()
        {
            return AttackButtonPressed();
        }

        #endregion

        #region IHitController Methods

        public bool HitDetected()
        {
            return MoleWasHit();
        }

        #endregion

        #region Unity Methods

        void Start()
        {
            
        }

        void Update()
        {

        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            if (playerController == null)
            {
                throw new PlayerControllerException();
            }

            playerController.SetInputController(this);
            playerController.SetHitController(this);
            HitCollisionId = -1;
        }

        public void ClearHitCollisionId()
        {
            HitCollisionId = -1;
        }

        #endregion

        #region Private Methods

        private bool AttackButtonPressed()
        {
            if(!ReceivedInput())
            {
                return false;
            }

            ++WhackAttempts;
            return true;
        }

        private bool ReceivedInput()
        {
            if (MouseInputReceived())
            {
                return true;
            }

            if(TouchInputReceived())
            {
                return true;
            }

            return false;
        }

        private bool MoleWasHit()
        {
            if (!IsInputOverMole())
            {
                return false;
            }

            ++Whacks;
            return true;
        }

        private bool IsInputOverMole()
        {
            if (PlatformSupportsMouseInput())
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (!hit)
                {
                    HitCollisionId = -1;
                    return false;
                }

                if (hit.collider == null)
                {
                    throw new PlayerException();
                }

                HitCollisionId = hit.collider.GetInstanceID();
                return true;
            }
            else
                if (PlatformSupportsTouchInput())
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);

                if (!hit)
                {
                    HitCollisionId = -1;
                    return false;
                }

                HitCollisionId = hit.collider.GetInstanceID();
                return true;
            }

            HitCollisionId = -1;
            return false;
        }

        private bool MouseInputReceived()
        {
            if (!PlatformSupportsMouseInput())
            {
                return false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }

            return false;
        }

        private bool PlatformSupportsMouseInput()
        {
            return Application.platform == RuntimePlatform.WindowsPlayer ||
                   Application.platform == RuntimePlatform.WindowsEditor;
        }

        private bool TouchInputReceived()
        {
            if (!PlatformSupportsTouchInput())
            {
                return false;
            }

            foreach (Touch touch in Input.touches)
            {
                if(FingerCount && touch.phase != TouchPhase.Ended)
                {
                    return false;
                }

                if (touch.phase != TouchPhase.Ended && 
                    touch.phase != TouchPhase.Canceled &&
                    !FingerCount)
                {
                    FingerCount = true;
                    return true;
                }
            }

            FingerCount = false;
            return false;
        }

        private bool PlatformSupportsTouchInput()
        {
            return Application.platform == RuntimePlatform.Android ||
                   Application.platform == RuntimePlatform.IPhonePlayer;
        }

        #endregion
    }
}