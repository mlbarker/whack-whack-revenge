//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Hud
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Assets.Scripts.Level;
    using Assets.Scripts.Game;
    using Assets.Scripts.Persistence;

    public class Hud : MonoBehaviour
    {
        #region Private Members

        private Game m_game;
        private GameObject m_resultsWindow;
        private GameObject m_objectiveWindow;
        private GameObject[] m_filledStars;
        private List<Text> m_objectiveTexts;
        private Text m_scoreText;
        private Text m_whackPercentageText;
        private Text m_gameTimerText;
        private Text m_scoreResultText;
        private Text m_whackPercentageResultText;
        private int m_score;
        private int m_whackPercentage;
        private int m_gameTimer;
        private int m_starsAchievedCount;

        #endregion

        #region Unity Methods

        void Start()
        {
            Initialize();
        }

        void Update()
        {
            UpdateScore();
            UpdateGameTime();
            UpdateWhackPercentage();
            UpdateStarAchievements();
            DisplayGameResults();
        }

        #endregion

        #region Public Methods

        public void BackToLevelSelect()
        {
            SavePersistence();
            Application.LoadLevel(SceneIndices.LevelSelectScene);
        }

        public void ContinueToNextLevel()
        {
            SavePersistence();

            int levelId = LevelManager.Instance.GetNextLevelSelected();
            Application.LoadLevel(levelId);
        }

        public void CloseObjectiveWindow()
        {
            m_objectiveWindow.SetActive(false);
            m_game.DisplayObjectives = false;
            m_game.StartGameCalled = true;
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            InitializeStars();
            InitializeHudElements();
            InitializeResultsWindow();
            InitializeGameController();
            InitializeObjectiveWindow();
        }

        private void InitializeGameController()
        {
            m_game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        }

        private void InitializeHudElements()
        {
            m_scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
            m_gameTimerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
            m_whackPercentageText = GameObject.FindGameObjectWithTag("Percentage").GetComponent<Text>();
            m_scoreResultText = GameObject.FindGameObjectWithTag("ScoreResult").GetComponent<Text>();
            m_whackPercentageResultText = GameObject.FindGameObjectWithTag("PercentageResult").GetComponent<Text>();
        }

        private void InitializeObjectiveWindow()
        {
            GameObject[] objectiveObjects = GameObject.FindGameObjectsWithTag("ObjectiveText");
            m_objectiveTexts = new List<Text>();
            for (int index = 0; index < objectiveObjects.Length; ++index)
            {
                m_objectiveTexts.Add(objectiveObjects[index].GetComponent<Text>());
            }

            m_objectiveWindow = GameObject.FindGameObjectWithTag("ObjectiveWindow");
            SetObjectives();
        }

        private void InitializeResultsWindow()
        {
            m_resultsWindow = GameObject.FindGameObjectWithTag("ResultsWindow");
            m_resultsWindow.SetActive(false);
        }

        private void InitializeStars()
        {
            m_filledStars = GameObject.FindGameObjectsWithTag("StarFilled");

            for(int index = 0; index < m_filledStars.Length; ++index)
            {
                m_filledStars[index].SetActive(false);
            }
        }

        private void UpdateScore()
        {
            if(GameIsFinished())
            {
                return;
            }

            m_score = m_game.Score;
            m_scoreText.text = m_score.ToString();
        }

        private void UpdateGameTime()
        {
            if (GameIsFinished())
            {
                return;
            }

            m_gameTimer = m_game.GameTimeSecondsLeft;
            m_gameTimerText.text = m_gameTimer.ToString();
        }

        private void UpdateWhackPercentage()
        {
            if (GameIsFinished())
            {
                return;
            }

            float percent = m_game.WhackPercentage;
            m_whackPercentage = (int)percent;
            m_whackPercentageText.text = m_whackPercentage.ToString() + "%";
        }

        private void UpdateStarAchievements()
        {
            if(GameIsFinished())
            {
                return;
            }

            if(m_starsAchievedCount == m_game.StarsAchievedCount)
            {
                return;
            }

            if(m_starsAchievedCount < m_game.StarsAchievedCount)
            {
                m_filledStars[m_starsAchievedCount].SetActive(true);
                ++m_starsAchievedCount;
                return;
            }

            if(m_starsAchievedCount > m_game.StarsAchievedCount)
            {
                --m_starsAchievedCount;
                m_filledStars[m_starsAchievedCount].SetActive(false);
                return;
            }
        }

        private void SetObjectives()
        {
            for (int index = 0; index < m_objectiveTexts.Count; ++index)
            {
                m_objectiveTexts[index].text = m_game.Objectives[index];
            }
        }

        private void DisplayGameResults()
        {
            m_scoreResultText.text = "Final Score - " + m_scoreText.text;
            m_whackPercentageResultText.text = "Final Whack % - " + m_whackPercentageText.text;

            bool active = m_game.DisplayGameResults;
            m_resultsWindow.SetActive(active);
        }

        private bool GameIsFinished()
        {
            return m_game.DisplayGameResults;
        }

        private void SavePersistence()
        {
            if(!PersistentManager.ModifiedData)
            {
                return;
            }

            PersistentManager.Instance.Save(Application.persistentDataPath);
        }

        #endregion
    }
}
