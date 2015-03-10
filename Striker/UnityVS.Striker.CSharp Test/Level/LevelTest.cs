//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace UnityVS.Striker.CSharp_Test.Level
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;

    [TestClass]
    public class LevelTest
    {
        #region Test Members

        ILevelManager m_levelManager;
        ILevelZone m_levelZone;
        ILevel m_level;

        #endregion

        [TestInitialize]
        public void InitializeTest()
        {
            m_levelManager = new LevelManager();
            m_levelZone = new LevelZone();
            m_level = new Level();
        }

        [TestCleanup]
        public void CleanUp()
        {
            m_levelManager = null;
            m_levelZone = null;
            m_level = null;
        }

        [TestMethod]
        public void LevelZoneIdInLevelManagerTest()
        {
            m_levelManager.AddZone(m_levelZone, LevelZoneId.Plain);

            bool actualResult = m_levelManager.ContainsZone(LevelZoneId.Plain);
            Assert.IsTrue(actualResult);
        }

        [TestMethod]
        public void LevelManagerContainsLevel1Test()
        {
            m_levelManager.AddZone(m_levelZone, LevelZoneId.Plain);
            m_levelManager.AddLevelToZone(LevelZoneId.Plain, LevelId.One, m_level);

            bool actualResult = m_levelManager.ContainsLevel(LevelZoneId.Plain, LevelId.One);
            Assert.IsTrue(actualResult);
        }
    }
}
