//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Level
{
    using System;
    using Assets.Scripts.Interfaces;

    [Serializable]
    public class LevelInfo : IDisposable
    {
        #region Fields

        bool m_disposed = false;

        #endregion

        #region Public Properties

        public LevelZoneId ZoneId
        {
            get;
            private set;
        }

        public LevelId LevelIdNum
        {
            get;
            private set;
        }

        public LevelStarInfo[] LevelStarInfos
        {
            get;
            private set;
        }

        public int LevelTimeInSeconds
        {
            get;
            private set;
        }

        #endregion

        #region IDisposable Methods

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion

        #region Constructors

        public LevelInfo(LevelZoneId zoneId, LevelId levelId, LevelStarInfo[] starInfos, int levelTimeInSeconds)
        {
            ZoneId = zoneId;
            LevelIdNum = levelId;
            LevelStarInfos = starInfos;
            LevelTimeInSeconds = levelTimeInSeconds;

            m_disposed = false;
        }

        private LevelInfo()
        {
        }

        #endregion

        #region Private Methods

        private void Dispose(bool disposing)
        {
            if(m_disposed)
            {
                return;
            }

            ZoneId = LevelZoneId.INVALID;
            LevelIdNum = LevelId.INVALID;
            LevelTimeInSeconds = -1;

            LevelStarInfos[0] = null;
            LevelStarInfos[1] = null;
            LevelStarInfos[2] = null;

            m_disposed = true;
        }

        #endregion
    }
}
