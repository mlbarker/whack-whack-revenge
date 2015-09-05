//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Level
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Assets.Scripts.Game;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Level;
    using Assets.Scripts.Persistence;

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
                    // TODO: update the panel status
                    StoredSelections.SetSelection(levelPanel.name, false);
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
            StoreLevelInfoData();
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
            //foreach (GameObject levelButtonObject in levelButtons)
            //{
            //    Button button = levelButtonObject.GetComponent<Button>();
            //    m_levelButtons.Add(button);
            //}

            for (int count = 0; count < levelButtons.Length; ++count)
            {
                Button button = levelButtons[count].GetComponent<Button>();
                m_levelButtons.Add(button);
            }
        }

        private void InitializeLevelManager()
        {
            LevelManager.Instance.Clear();

            for (int count = 0; count < m_zoneButtons.Count; ++count)
            {
                var zoneButton = m_zoneButtons[count];
                var levelPanel = m_levelPanels[count];

                Zone zone = zoneButton.GetComponent<Zone>();
                zoneButton.onClick.AddListener(() => OnZoneSelected(zone, levelPanel));
                // TODO: store the levelPanel name here and check if it was last active
                if (ZonePreviouslySelected(levelPanel.name))
                {
                    levelPanel.SetActive(true);
                }
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
                bool selected = StoredSelections.GetSelection(levelPanel.name);
                levelPanel.SetActive(selected);
            }

            if(LevelManager.Instance.GoToNextLevel)
            {
                LevelZoneId zoneId =LevelManager.Instance.SelectedLevelInfo.ZoneId;
                LevelId levelId = LevelManager.Instance.SelectedLevelInfo.LevelIdNum + 1;

                LevelManager.Instance.NextLevelLoaded();
                GoToSelectedLevel(zoneId, levelId);
            }
        }

        private bool ZonePreviouslySelected(string zoneName)
        {
            return StoredSelections.GetSelection(zoneName);
        }

        private void ClearLevels()
        {
            LevelManager.Instance.Clear();
        }

        private void OnZoneSelected(Zone zone, GameObject panelObject)
        {
            if (!IsZoneUnlocked(zone.RequiredStars))
            {
                Debug.LogWarning("Zone locked!");
                return;
            }

            panelObject.SetActive(true);
            // TODO: check the name and store the panel's active status
            StoredSelections.SetSelection(panelObject.name, true);
        }

        private void OnLevelSelected(Button button)
        {
            LevelZoneId zoneId = button.GetComponent<Level>().zoneId;
            LevelId levelId = button.GetComponent<Level>().levelId;

            GoToSelectedLevel(zoneId, levelId);
        }

        private void GoToSelectedLevel(LevelZoneId zoneId, LevelId levelId)
        {
            // check if level is unlocked
            if (!IsLevelUnlocked(zoneId, levelId))
            {
                Debug.LogWarning("Level is locked!");
                return;
            }

            // clear the pause manager of the values collected
            PauseManager.Instance.Clear();

            // need the star types prior to starting the game
            SaveLevelStarTypes((int)zoneId, (int)levelId);

            LevelManager.Instance.StoreSelectedLevelInfo(zoneId, levelId);
            Application.LoadLevel((int)levelId);
        }

        private void SaveLevelStarTypes()
        {
            for (int zoneIndex = 0; zoneIndex < (int)LevelZoneId.MaxZones; ++zoneIndex)
            {
                for (int levelIndex = 0; levelIndex < LevelZone.MAX_LEVELS; ++levelIndex)
                {
                    // offset for level IDs
                    int levelKey = (zoneIndex * (int)LevelZone.MAX_LEVELS) + (int)LevelId.Plains1;

                    LevelInfo levelInfo = LevelManager.Instance.GetLevelInfo((LevelZoneId)zoneIndex, (LevelId)levelKey);

                    PersistentManager.Instance.SetValue(zoneIndex, levelKey, DataIndex.Star1Type, (int)levelInfo.LevelStarInfos[0].StarType);
                    PersistentManager.Instance.SetValue(zoneIndex, levelKey, DataIndex.Star2Type, (int)levelInfo.LevelStarInfos[1].StarType);
                    PersistentManager.Instance.SetValue(zoneIndex, levelKey, DataIndex.Star3Type, (int)levelInfo.LevelStarInfos[2].StarType);
                }
            }
        }

        private void SaveLevelStarTypes(int zoneId, int levelId)
        {
            LevelInfo levelInfo = LevelManager.Instance.GetLevelInfo((LevelZoneId)zoneId, (LevelId)levelId);

            PersistentManager.Instance.SetValue(zoneId, levelId, DataIndex.Star1Type, (int)levelInfo.LevelStarInfos[0].StarType);
            PersistentManager.Instance.SetValue(zoneId, levelId, DataIndex.Star2Type, (int)levelInfo.LevelStarInfos[1].StarType);
            PersistentManager.Instance.SetValue(zoneId, levelId, DataIndex.Star3Type, (int)levelInfo.LevelStarInfos[2].StarType);
        }

        private bool IsZoneUnlocked(int requiredStars)
        {
            int playerKey = PersistentManager.PlayerKey;
            int starsCollected = PersistentManager.Instance.GetValue(playerKey, playerKey, DataIndex.StarsCollected);
            
            bool unlockedZone = (starsCollected >= requiredStars) ? true : false;
            return unlockedZone;
        }

        private bool IsLevelUnlocked(LevelZoneId zoneId, LevelId levelId)
        {
            int unlocked = PersistentManager.Instance.GetValue((int)zoneId, (int)levelId, DataIndex.Unlocked);

            bool unlockedLevel = unlocked == 1 ? true : false;
            return unlockedLevel;
        }

        private void StoreLevelInfoData()
        {
            for (int zoneIndex = 0; zoneIndex < (int)LevelZoneId.MaxZones; ++zoneIndex)
            {
                int maxLevels = LevelZone.MAX_LEVELS * (zoneIndex + 1);
                maxLevels += (int)LevelId.Plains1;

                int levelIndex = (int)LevelId.Plains1 + (LevelZone.MAX_LEVELS * zoneIndex);
                for (; levelIndex < maxLevels; ++levelIndex)
                {
                    LevelInfo levelInfo = LevelManager.Instance.GetLevelInfo((LevelZoneId)zoneIndex, (LevelId)levelIndex);
                    PersistentManager.Instance.StoreLevelInfoData(zoneIndex, levelIndex, levelInfo);
                }
            }
        }

        #endregion
    }
}
