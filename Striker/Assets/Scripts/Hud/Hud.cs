//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Hud
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Assets.Scripts.Game;

    public class Hud : MonoBehaviour
    {
        #region Private Members

        private Game m_game;
        private GameObject m_resultsWindow;
        private Text m_scoreText;
        private Text m_whackPercentageText;
        private Text m_gameTimerText;
        private Text m_scoreResultText;
        private Text m_whackPercentageResultText;
        private int m_score;
        private int m_whackPercentage;
        private int m_gameTimer;

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
            DisplayGameResults();
        }

        #endregion

        #region Public Methods

        public void BackToMainMenu()
        {
            Application.LoadLevel(0);
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            m_scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
            m_gameTimerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
            m_whackPercentageText = GameObject.FindGameObjectWithTag("Percentage").GetComponent<Text>();
            m_scoreResultText = GameObject.FindGameObjectWithTag("ScoreResult").GetComponent<Text>();
            m_whackPercentageResultText = GameObject.FindGameObjectWithTag("PercentageResult").GetComponent<Text>();
            m_resultsWindow = GameObject.FindGameObjectWithTag("ResultsWindow");
            m_game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();

            m_resultsWindow.SetActive(false);
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

        #endregion
    }
}
