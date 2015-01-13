//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Player
{
    using System;
    using UnityEngine;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Player;

    public class Player : MonoBehaviour, IInputController
    {
        #region Public Properties

        public int HitCollisionId
        {
            get;
            private set;
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

            playerController.Initialize();
            playerController.SetInputController(this);
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

            if(!IsInputOverMole())
            {
                return false;
            }

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
            else if (PlatformSupportsTouchInput())
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
            if(!PlatformSupportsMouseInput())
            {
                return false;
            }

            if(Input.GetMouseButtonDown(0))
            {
                Debug.Log("WHACK!");
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

            int fingerCount = 0;
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                {
                    fingerCount++;
                    return true;
                }
            }

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
