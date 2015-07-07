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
            LevelManager.Instance.Clear();

            for (int count = 0; count < m_zoneButtons.Count; ++count)
            {
                var zoneButton = m_zoneButtons[count];
                var levelPanel = m_levelPanels[count];

                Zone zone = zoneButton.GetComponent<Zone>();
                zoneButton.onClick.AddListener(() => OnZoneSelected(zone, levelPanel));
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
        }

        private void OnLevelSelected(Button button)
        {
            LevelZoneId zoneId = button.GetComponent<Level>().zoneId;
            LevelId levelId = button.GetComponent<Level>().levelId;

            // check if level is unlocked
            if (!IsLevelUnlocked(zoneId, levelId))
            {
                Debug.LogWarning("Level is locked!");
                return;
            }

            // clear the pause manager of the values collected
            PauseManager.Instance.Clear();

            // need the star types prior to starting the game
            SaveLevelStarTypes();

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
                    int levelKey = (int)LevelId.Plains1 + levelIndex;
                    levelKey += (zoneIndex * (int)LevelZone.MAX_LEVELS);

                    LevelDataBlock block = PersistentManager.Instance.GetDataBlock(zoneIndex, levelKey) as LevelDataBlock;
                    if(block == null)
                    {
                        return;
                    }

                    LevelInfo levelInfo = LevelManager.Instance.GetLevelInfo((LevelZoneId)zoneIndex, (LevelId)levelKey);

                    block.StoreValues(DataIndex.Star1Type, (int)levelInfo.levelStarInfos[0].starType);
                    block.StoreValues(DataIndex.Star2Type, (int)levelInfo.levelStarInfos[1].starType);
                    block.StoreValues(DataIndex.Star3Type, (int)levelInfo.levelStarInfos[2].starType);

                    PersistentManager.Instance.AddBlock(zoneIndex, levelKey, block);
                }
            }
        }

        private bool IsZoneUnlocked(int requiredStars)
        {
            int playerKey = PersistentManager.PlayerKey;
            IDataBlock dataBlock = PersistentManager.Instance.GetDataBlock(playerKey, playerKey);
            if (dataBlock == null)
            {
                Debug.Log("DataBlock is null");
                return false;
            }

            List<int> values = dataBlock.GetValues();
            int starsIndex = (int)DataIndex.StarsCollected;
            bool unlockedZone = (values[starsIndex] >= requiredStars) ? true : false;
            return unlockedZone;
        }

        private bool IsLevelUnlocked(LevelZoneId zoneId, LevelId levelId)
        {
            IDataBlock dataBlock = PersistentManager.Instance.GetDataBlock((int)zoneId, (int)levelId);
            if (dataBlock == null)
            {
                Debug.Log("DataBlock is null");
                return false;
            }

            List<int> values = dataBlock.GetValues();
            int unlockedIndex = (int)DataIndex.Unlocked;
            bool unlockedLevel = values[unlockedIndex] == 1 ? true : false;
            return unlockedLevel;
        }

        #endregion
    }
}
