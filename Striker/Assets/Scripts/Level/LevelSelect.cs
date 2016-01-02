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
        private List<Text> m_totalStarsInZone;
        private Text m_totalStarsInGame;
        private int m_totalStarsAchieved;

        #endregion

        #region Unity Methods

        void Start()
        {
            Initialize();
        }

        void Update()
        {
            BackButtonPressed();
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
            InitializeTotalStarsTexts();
            InitializeZoneButtons();
            InitializeLevelPanels();
            InitializeLevelButtons();
            InitializeLevelManager();
            InitializeZoneLocks();
            InitializeLevelLocks();
            InitializeLevelStarsAchieved();
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

        private void InitializeTotalStarsTexts()
        {
            GameObject totalStarsInGame = GameObject.FindGameObjectWithTag("TotalStarsGame");
            m_totalStarsInGame = totalStarsInGame.GetComponent<Text>();

            int playerKey = PersistentManager.PlayerKey;
            int totalStars = PersistentManager.Instance.GetValue(playerKey, playerKey, DataIndex.StarsCollected);
            m_totalStarsInGame.text += totalStars.ToString();

            GameObject[] totalStarsInZones = GameObject.FindGameObjectsWithTag("TotalStarsZone");
            m_totalStarsInZone = new List<Text>();
            foreach (GameObject starsInZone in totalStarsInZones)
            {
                Text starsInZoneText = starsInZone.GetComponent<Text>();
                m_totalStarsInZone.Add(starsInZoneText);
            }

            for (int index = 0; index < (int)LevelZoneId.MaxZones; ++index)
            {
                LevelZoneId zone = m_totalStarsInZone[index].GetComponent<ZoneDetermination>().zone;

                int starsAchieved = 0;
                int totalStarsToAchieve = Level.MAX_STARS * LevelZone.MAX_LEVELS;
                // 
                int maxLevels = LevelZone.MAX_LEVELS * ((int)zone + 1);
                maxLevels += (int)LevelId.Plains1;

                int levelIndex = (int)LevelId.Plains1 + (LevelZone.MAX_LEVELS * (int)zone);
                for (; levelIndex < maxLevels; ++levelIndex)
                {
                    starsAchieved += PersistentManager.Instance.GetValue((int)zone, levelIndex, DataIndex.Star1Achieved);
                    starsAchieved += PersistentManager.Instance.GetValue((int)zone, levelIndex, DataIndex.Star2Achieved);
                    starsAchieved += PersistentManager.Instance.GetValue((int)zone, levelIndex, DataIndex.Star3Achieved);
                }

                m_totalStarsInZone[index].text = starsAchieved.ToString() + "/" + totalStarsToAchieve.ToString();
                m_totalStarsAchieved += starsAchieved;
            }
        }

        private void InitializeZoneLocks()
        {
            for (int index = 0; index < m_zoneButtons.Count; ++index)
            {
                Zone zone = m_zoneButtons[index].GetComponent<Zone>();
                bool unlocked = IsZoneUnlocked(zone.RequiredStars);

                Transform zoneTransform = m_zoneButtons[index].transform;
                for (int childCount = 0; childCount < zoneTransform.childCount; ++childCount)
                {
                    Transform child = zoneTransform.GetChild(childCount);
                    if (child.CompareTag("ZoneLock"))
                    {
                        child.gameObject.SetActive(!unlocked);
                    }
                    else
                    {
                        Debug.LogWarning("Not zone lock object");
                    }
                }
            }
        }

        private void InitializeLevelLocks()
        {
            for (int index = 0; index < m_levelButtons.Count; ++index)
            {
                Level level = m_levelButtons[index].GetComponent<Level>();
                bool unlocked = IsLevelUnlocked(level.zoneId, level.levelId);

                Transform levelTransform = m_levelButtons[index].transform;
                for (int childCount = 0; childCount < levelTransform.childCount; ++childCount)
                {
                    Transform child = levelTransform.GetChild(childCount);
                    if (child.CompareTag("LevelLock"))
                    {
                        child.gameObject.SetActive(!unlocked);
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("Not level lock object");
                    }
                }
            }
        }

        private void InitializeLevelStarsAchieved()
        {
            for (int index = 0; index < m_levelButtons.Count; ++index)
            {
                Level level = m_levelButtons[index].GetComponent<Level>();
                bool star1Achieved = PersistentManager.Instance.GetValue((int)level.zoneId, (int)level.levelId, DataIndex.Star1Achieved) == 1 ? true : false;
                bool star2Achieved = PersistentManager.Instance.GetValue((int)level.zoneId, (int)level.levelId, DataIndex.Star2Achieved) == 1 ? true : false;
                bool star3Achieved = PersistentManager.Instance.GetValue((int)level.zoneId, (int)level.levelId, DataIndex.Star3Achieved) == 1 ? true : false;

                Transform levelTransform = m_levelButtons[index].transform;
                for (int childCount = 0; childCount < levelTransform.childCount; ++childCount)
                {
                    Transform child = levelTransform.GetChild(childCount);
                    if (child.CompareTag("Star1Achieved"))
                    {
                        child.gameObject.SetActive(star1Achieved);
                        continue;
                    }
                    else if (child.CompareTag("Star2Achieved"))
                    {
                        child.gameObject.SetActive(star2Achieved);
                        continue;
                    }
                    else if (child.CompareTag("Star3Achieved"))
                    {
                        child.gameObject.SetActive(star3Achieved);
                        continue;
                    }
                    else
                    {
                        Debug.LogWarning("Not star achieved object");
                    }
                }
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

        private void BackButtonPressed()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnBackButtonPressed();
            }
        }

        #endregion
    }
}
