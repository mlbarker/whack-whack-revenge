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

        private Button zonePlain;
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

        public void OnPlainZoneSelected()
        {
            levelPanel.SetActive(true);
        }

        public void OnBackButtonPressed()
        {
            if(levelPanel.activeSelf)
            {
                levelPanel.SetActive(false);
            }
            else
            {
                // TODO: need something better than this...
                Application.LoadLevel(0);
            }
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            // I need to grab the zone and a level in the zone.
            // The issue is that the level name/number (which does not exist as a data member
            // in the level interface) must correspond to the scene order in the build settings.
            zonePlain = GameObject.FindGameObjectWithTag("ZonePlain").GetComponent<Button>();
            levelPanel = GameObject.FindGameObjectWithTag("LevelPanel");
            levelObject = GameObject.FindGameObjectWithTag("Level");

            if(zonePlain == null)
            {
                return;
            }

            if(levelPanel == null)
            {
                return;
            }

            level = levelPanel.GetComponentInChildren<Button>();
            Level levelOne = level.GetComponent<Level>();
            LevelManager.Instance.AddZone(new LevelZone(), LevelZoneId.Plain);
            LevelManager.Instance.AddLevelToZone(LevelZoneId.Plain, LevelId.One, levelOne);

            levelPanel.SetActive(false);
        }

        #endregion
    }
}
