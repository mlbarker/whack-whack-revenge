//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Menu
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;
    using Assets.Scripts.Level;
    using Assets.Scripts.Interfaces;

    public class Menu : MonoBehaviour, IMenuNavigationController
    {
        #region Private Members

        private GameObject m_statsMenu;
        private GameObject m_optionsMenu;
        private Text m_totalScoreText;
        private Text m_totalWhackPercentageText;

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

        public void BackToPreviousMenu()
        {
            DeactivateStatsMenu();
            DeactivateOptionsMenu();
            m_menuController.BackToPreviousMenu();
        }

        public void RunMainMenu()
        {
            Application.LoadLevel(SceneIndices.MainMenuScene);
        }

        public void RunStartGame()
        {
            Application.LoadLevel(SceneIndices.LevelSelectScene);
        }

        public void RunStats()
        {
            UpdateStatsMenu();
        }

        public void RunOptions()
        {
            UpdateOptionsMenu();
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
            InitializeMenuController();
            InitializeStatsPanel();
            InitializeOptionsPanel();
        }

        private void InitializeMenuController()
        {
            if (m_menuController == null)
            {
                m_menuController = new MenuController();
            }

            m_menuController.SetMenuNavigationController(this);
            m_menuController.Initialize();
        }

        private void InitializeStatsPanel()
        {
            m_statsMenu = GameObject.FindGameObjectWithTag("StatsMenuPanel");
            m_totalScoreText = GameObject.FindGameObjectWithTag("TotalWhackScore").GetComponent<Text>();
            m_totalWhackPercentageText = GameObject.FindGameObjectWithTag("TotalWhackPercentage").GetComponent<Text>();

            m_statsMenu.SetActive(false);
        }

        private void InitializeOptionsPanel()
        {
            m_optionsMenu = GameObject.FindGameObjectWithTag("OptionsMenuPanel");

            m_optionsMenu.SetActive(false);
        }

        private void UpdateStatsMenu()
        {
            m_totalScoreText.text = "Total Score: ";
            m_totalWhackPercentageText.text = "Total Whack %: ";
            m_statsMenu.SetActive(true);
        }

        private void UpdateOptionsMenu()
        {
            m_optionsMenu.SetActive(true);
        }

        private void DeactivateStatsMenu()
        {
            m_statsMenu.SetActive(false);
        }

        private void DeactivateOptionsMenu()
        {
            m_optionsMenu.SetActive(false);
        }

        #endregion
    }
}
