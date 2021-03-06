﻿//-----------------------------
// ImperfectlyCoded © 2016
//-----------------------------

namespace Assets.Scripts.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Utilities.Timers;

    public class EndGameManager
    {
        #region Constants

        private const int MAX_END_GAME_TIME = 2;

        #endregion

        #region Fields

        private static EndGameManager m_instance;
        private Dictionary<int, IEndGame> m_endGameObjects = new Dictionary<int, IEndGame>();
        private Timer m_endGameTimer;

        #endregion

        #region Public Properties

        public static EndGameManager Instance
        {
            get
            {
                if(m_instance == null)
                {
                    m_instance = new EndGameManager();
                }

                return m_instance;
            }
        }

        public bool IsEndGameActive
        {
            get;
            private set;
        }

        public bool IsEndGameTimeDone
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        private EndGameManager()
        {
            IsEndGameTimeDone = false;
            IsEndGameActive = false;
            m_endGameTimer = new Timer(MAX_END_GAME_TIME, EndGameIsDone);
        }

        #endregion

        #region Public Methods

        public void Add(int hashCode, IEndGame endGameObject)
        {
            if(!m_endGameObjects.ContainsKey(hashCode))
            {
                m_endGameObjects.Add(hashCode, endGameObject);
            }

            m_endGameTimer.ResetTimer();
        }

        public void Remove(int hashCode)
        {
            if(m_endGameObjects.ContainsKey(hashCode))
            {
                m_endGameObjects.Remove(hashCode);
            }
        }

        public void Clear()
        {
            IsEndGameTimeDone = false;
            IsEndGameActive = false;
            m_endGameTimer.StopTimer();
            m_endGameTimer.ResetTimer();
            m_endGameObjects.Clear();
        }

        public void RunEndGame(bool playerDefeated)
        {
            foreach (KeyValuePair<int, IEndGame> endGameObject in m_endGameObjects)
            {
                endGameObject.Value.OnEndGame(playerDefeated);
            }
            
            m_endGameTimer.StartTimer();
            IsEndGameActive = true;
        }

        public void Update()
        {
            if(m_endGameTimer.Active())
            {
                m_endGameTimer.Update();
            }
            else
            {
                IsEndGameActive = false;
            }
        }

        #endregion

        #region Private Methods

        private void EndGameIsDone()
        {
            IsEndGameTimeDone = true;
        }

        #endregion
    }
}
