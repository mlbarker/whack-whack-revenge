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
        private Dictionary<Type, IPauseController> m_pauseControllers = new Dictionary<Type, IPauseController>();

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

        public void Add(Type type, IPauseController pauseController)
        {
            if(m_pauseControllers.ContainsKey(type))
            {
                return;
            }

            m_pauseControllers.Add(type, pauseController);
        }

        public void Remove(Type type)
        {
            if(!m_pauseControllers.ContainsKey(type))
            {
                return;
            }

            m_pauseControllers.Remove(type);
        }

        public void GamePaused()
        {
            foreach (Type type in m_pauseControllers.Keys)
            {
                m_pauseControllers[type].OnGamePaused();
            }
        }

        public void GameResumed()
        {
            foreach (Type type in m_pauseControllers.Keys)
            {
                m_pauseControllers[type].OnGameResumed();
            }
        }

        #endregion
    }
}
