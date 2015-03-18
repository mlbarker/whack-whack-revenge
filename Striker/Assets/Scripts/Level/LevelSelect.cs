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
        private Dictionary<Button, GameObject> m_zonesAndLevels = new Dictionary<Button, GameObject>();
        private Button level;
        private GameObject levelPanel;
        private GameObject levelObject;

        #endregion

        #region Unity Methods

        void Start()
        {
            Initialize();
        }

        #endregion

        #region Public Methods

        public void OnZoneSelected(GameObject go)
        {
            go.SetActive(true);
        }

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
                Application.LoadLevel(LeveZoneIndices.MainMenuScene);
            }
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            // I need to grab the zone and a level in the zone.
            // The issue is that the level name/number (which does not exist as a data member
            // in the level interface) must correspond to the scene order in the build settings.
            //zonePlain = GameObject.FindGameObjectWithTag("ZonePlain").GetComponent<Button>();
            //levelPanel = GameObject.FindGameObjectWithTag("LevelPanel");
            //levelObject = GameObject.FindGameObjectWithTag("Level");
            m_zoneButtons = new List<Button>();
            GameObject[] zoneObjects = GameObject.FindGameObjectsWithTag("Zone");
            foreach (GameObject zoneGameObject in zoneObjects)
            {
                m_zoneButtons.Add(zoneGameObject.GetComponent<Button>());
            }

            m_levelPanels = new List<GameObject>();
            GameObject[] levelPanelObjects = GameObject.FindGameObjectsWithTag("LevelPanel");
            m_levelPanels.AddRange(levelPanelObjects);

            foreach(GameObject levelPanel in m_levelPanels)
            {
                levelPanel.SetActive(false);
            }
            //for (int count = 0; count < m_zoneButtons.Count; ++count)
            //{
            //    m_zoneButtons[count].onClick.AddListener(() => OnZoneSelected(m_levelPanels[count]));
            //}


            //m_zoneButtons[0]
            m_zonesAndLevels.Add(m_zoneButtons[0], m_levelPanels[0]);
            m_zonesAndLevels.Add(m_zoneButtons[1], m_levelPanels[1]);

            //foreach (KeyValuePair<Button, GameObject> zoneAndLevels in m_zonesAndLevels)
            //{
            //    zoneAndLevels.Key.onClick.AddListener(() => OnZoneSelected(zoneAndLevels.Value));
            //}

            // this works.... sigh.... can't loop to set this up... oh well...
            m_zoneButtons[0].onClick.AddListener(() => OnZoneSelected(m_levelPanels[0]));
            m_zoneButtons[1].onClick.AddListener(() => OnZoneSelected(m_levelPanels[1]));

            //level = levelPanel.GetComponentInChildren<Button>();
            //Level levelOne = level.GetComponent<Level>();
            //LevelManager.Instance.AddZone(new LevelZone(), LevelZoneId.Plain);
            //LevelManager.Instance.AddLevelToZone(LevelZoneId.Plain, LevelId.One, levelOne);

            //levelPanel.SetActive(false);
        }

        #endregion
    }
}
