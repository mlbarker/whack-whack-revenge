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
        //#region Private Members

        //private Game m_game;
        //private Text m_scoreText;
        //private Text m_whackPercentageText;
        //private Text m_gameTimerText;
        //private int m_score;
        //private int m_whackPercentage;
        //private int m_gameTimer;

        //#endregion

        //#region Unity Methods

        //void Start()
        //{
        //    Initialize();
        //}

        //void Update()
        //{
        //    UpdateScore();
        //    UpdateGameTime();
        //    UpdateWhackPercentage();
        //}

        //#endregion

        //#region Public Methods

        //public void BackToMainMenu()
        //{
        //    Application.LoadLevel(0);
        //}

        //#endregion

        //#region Private Methods

        //private void Initialize()
        //{
        //    m_scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        //    m_gameTimerText = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        //    m_whackPercentageText = GameObject.FindGameObjectWithTag("Percentage").GetComponent<Text>();
        //    m_game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        //}

        //private void UpdateScore()
        //{
        //    m_score = m_game.Score;
        //    m_scoreText.text = m_score.ToString();
        //}

        //private void UpdateGameTime()
        //{
        //    m_gameTimer = m_game.GameTimeSecondsLeft;
        //    m_gameTimerText.text = m_gameTimer.ToString();
        //}

        //private void UpdateWhackPercentage()
        //{
        //    float percent = m_game.WhackPercentage * 100;
        //    m_whackPercentage = (int)percent;
        //    m_whackPercentageText.text = m_whackPercentage.ToString() + "%";
        //}

        //#endregion
    }
}
