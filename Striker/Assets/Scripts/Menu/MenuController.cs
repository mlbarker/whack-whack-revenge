//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Menu
{
    using System.Collections.Generic;
    using Assets.Scripts.Interfaces;

    public class MenuController
    {
        #region Private Members

        private Dictionary<MenuState, RunMenuState> m_menus = new Dictionary<MenuState,RunMenuState>();
        private IMenuNavigationController m_menuNavigationController;

        #endregion

        #region Delegates

        private delegate void RunMenuState();

        #endregion

        #region Public Properties

        public MenuState CurrentMenuState
        {
            get;
            private set;
        }

        public MenuState PreviousMenuState
        {
            get;
            private set;
        }

        #endregion

        #region Public Methods

        public void Initialize()
        {
            SetupMenuStates();
        }

        public void SetMenuNavigationController(IMenuNavigationController menuNavigationController)
        {
            if(menuNavigationController == null)
            {
                throw new MenuNavigationControllerException();
            }

            m_menuNavigationController = menuNavigationController;
        }

        public void BackToPreviousMenu()
        {
            m_menus[PreviousMenuState]();
        }

        public void MainMenu()
        {
            m_menuNavigationController.RunMainMenu();
            CurrentMenuState = MenuState.Main;
            PreviousMenuState = MenuState.Exit;
        }

        public void StartGame()
        {
            m_menuNavigationController.RunStartGame();
            CurrentMenuState = MenuState.GameStart;
            PreviousMenuState = MenuState.Main;
        }

        public void OptionsMenu()
        {
            m_menuNavigationController.RunOptions();
            CurrentMenuState = MenuState.Options;
            PreviousMenuState = MenuState.Main;
        }

        public void StatsMenu()
        {
            m_menuNavigationController.RunStats();
            CurrentMenuState = MenuState.Stats;
            PreviousMenuState = MenuState.Main;
        }

        public void ExitApp()
        {
            m_menuNavigationController.RunExit();
            CurrentMenuState = MenuState.Exit;
        }

        public bool CheckCurrentMenuState(MenuState menuState)
        {
            return CurrentMenuState == menuState;
        }

        #endregion

        #region Private Methods

        private void SetupMenuStates()
        {
            m_menus.Add(MenuState.Main, MainMenu);
            m_menus.Add(MenuState.GameStart, StartGame);
            m_menus.Add(MenuState.Options, OptionsMenu);
            m_menus.Add(MenuState.Stats, StatsMenu);
            m_menus.Add(MenuState.Exit, ExitApp);
        }

        #endregion
    }
}
