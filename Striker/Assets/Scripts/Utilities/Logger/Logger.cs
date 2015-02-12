//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Utilities.Logger
{
    using System.Collections.Generic;
    using System.IO;

    public static class Logger
    {
        #region Private Members

        private static string m_filename = "Striker.log";
        private static string m_debug = "DEBUG";
        private static string m_info = "INFO";
        private static string m_warning = "WARNING";
        private static Dictionary<LogLevel, string> m_logLevels;

        #endregion

        #region Constructors

        static Logger()
        {
            m_logLevels = new Dictionary<LogLevel, string>()
            {
                {LogLevel.DEBUG, m_debug},
                {LogLevel.INFO, m_info},
                {LogLevel.WARNING, m_warning}
            };
        }

        #endregion

        #region Public Methods

        public static void Log(LogLevel logLevel, object obj, string message)
        {
            string messageLog = m_logLevels[logLevel] + " | " + obj.ToString() + " | " + message;
            WriteMessage(messageLog);
        }

        #endregion

        #region Private Methods

        private static void WriteMessage(string message)
        {
            using (StreamWriter writer = new StreamWriter(m_filename, true))
            {
                writer.WriteLine(message);
            }
        }

        #endregion
    }
}
