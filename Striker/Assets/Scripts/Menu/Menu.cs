//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Menu
{
    using System;
    using UnityEngine;
    using Assets.Scripts.Interfaces;

    public class Menu : MonoBehaviour, IMenuNavigationController
    {
        #region Private Members
        #endregion

        #region Editor Values

        public MenuController m_menuController;

        #endregion

        #region Unity Methods

        void Start()
        {
            Initialize();
        }

        #endregion

        #region IMenuNavigationController Methods

        public void RunMainMenu()
        {
            Application.LoadLevel(0);
        }

        public void RunStartGame()
        {
            Application.LoadLevel(3);
        }

        public void RunStats()
        {
            Application.LoadLevel(1);
        }

        public void RunOptions()
        {
            Application.LoadLevel(2);
        }

        public void RunExit()
        {
            Application.Quit();
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods

        private void Initialize()
        {
            if(m_menuController == null)
            {
                m_menuController = new MenuController();
            }

            m_menuController.SetMenuNavigationController(this);
            m_menuController.Initialize();
        }

        #endregion
    }
}
