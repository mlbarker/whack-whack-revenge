//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Game
{
    using System;
    using System.Collections.Generic;
    using Assets.Scripts.Interfaces;

    public class PauseManager
    {
        #region Fields

        private static PauseManager m_instance;
        private Dictionary<int, IPauseController> m_pauseControllers = new Dictionary<int, IPauseController>();

        #endregion

        #region Public Properties

        public static PauseManager Instance
        {
            get
            {
                if(m_instance == null)
                {
                    m_instance = new PauseManager();
                }

                return m_instance;
            }
        }

        #endregion

        #region Constructors

        private PauseManager()
        {
        }

        #endregion

        #region Public Methods

        public void Add(int hashCode, IPauseController pauseController)
        {
            if (m_pauseControllers.ContainsKey(hashCode))
            {
                return;
            }

            m_pauseControllers.Add(hashCode, pauseController);
        }

        public void Remove(int hashCode)
        {
            if (!m_pauseControllers.ContainsKey(hashCode))
            {
                return;
            }

            m_pauseControllers.Remove(hashCode);
        }

        public void GamePaused()
        {
            foreach (int hashCode in m_pauseControllers.Keys)
            {
                m_pauseControllers[hashCode].OnGamePaused();
            }
        }

        public void GameResumed()
        {
            foreach (int hashCode in m_pauseControllers.Keys)
            {
                m_pauseControllers[hashCode].OnGameResumed();
            }
        }

        public void Clear()
        {
            m_pauseControllers.Clear();
        }

        #endregion
    }
}
