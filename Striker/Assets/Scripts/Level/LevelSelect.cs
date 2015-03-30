//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Level
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Level;

    public class LevelSelect : MonoBehaviour
    {
        #region Private Members

        private List<Button> m_zoneButtons;
        private List<GameObject> m_levelPanels;
        private List<Button> m_levelButtons;

        #endregion

        #region Unity Methods

        void Start()
        {
            Initialize();
        }

        #endregion

        #region Public Methods

        public void OnBackButtonPressed()
        {
            bool panelWasActive = false;
            foreach(GameObject levelPanel in m_levelPanels)
            {
                if(levelPanel.activeSelf)
                {
                    levelPanel.SetActive(false);
                    panelWasActive = true;
                }
            }

            if(!panelWasActive)
            {
                Application.LoadLevel(SceneIndices.MainMenuScene);
            }
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            InitializeZoneButtons();
            InitializeLevelPanels();
            InitializeLevelButtons();
            InitializeLevelManager();
            
        }

        private void InitializeZoneButtons()
        {
            m_zoneButtons = new List<Button>();
            GameObject[] zoneObjects = GameObject.FindGameObjectsWithTag("Zone");
            foreach (GameObject zoneGameObject in zoneObjects)
            {
                m_zoneButtons.Add(zoneGameObject.GetComponent<Button>());
            }
        }

        private void InitializeLevelPanels()
        {
            m_levelPanels = new List<GameObject>();
            GameObject[] levelPanelObjects = GameObject.FindGameObjectsWithTag("LevelPanel");
            m_levelPanels.AddRange(levelPanelObjects);
        }

        private void InitializeLevelButtons()
        {
            m_levelButtons = new List<Button>();
            GameObject[] levelButtons = GameObject.FindGameObjectsWithTag("Level");
            foreach (GameObject levelButtonObject in levelButtons)
            {
                Button button = levelButtonObject.GetComponent<Button>();
                m_levelButtons.Add(button);
            }
        }

        private void InitializeLevelManager()
        {
            for (int count = 0; count < m_zoneButtons.Count; ++count)
            {
                var zoneButton = m_zoneButtons[count];
                var levelPanel = m_levelPanels[count];

                zoneButton.onClick.AddListener(() => OnZoneSelected(levelPanel));
            }

            LevelManager.Instance.AddZone(new LevelZone(), LevelZoneId.Plain);
            LevelManager.Instance.AddZone(new LevelZone(), LevelZoneId.Sports);
            foreach (Button levelButton in m_levelButtons)
            {
                var button = levelButton;
                button.onClick.AddListener(() => OnLevelSelected(button));
                LevelManager.Instance.AddLevelToZone(button.GetComponent<Level>().zoneId,
                                                     button.GetComponent<Level>().levelId,
                                                     button.GetComponent<Level>());
            }

            foreach (GameObject levelPanel in m_levelPanels)
            {
                levelPanel.SetActive(false);
            }
        }

        private void OnZoneSelected(GameObject panelObject)
        {
            panelObject.SetActive(true);
        }

        private void OnLevelSelected(Button button)
        {
            LevelZoneId zoneId = button.GetComponent<Level>().zoneId;
            LevelId levelId = button.GetComponent<Level>().levelId;

            LevelManager.Instance.StoreSelectedLevelInfo(zoneId, levelId);
            Application.LoadLevel((int)levelId);
        }

        #endregion
    }
}
