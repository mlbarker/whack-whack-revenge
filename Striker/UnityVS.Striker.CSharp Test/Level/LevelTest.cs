//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace UnityVS.Striker.CSharp_Test.Level
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Level;

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
            m_levelManager = LevelManager.Instance;
            m_levelZone = new LevelZone();
            m_level = new Level();
        }

        [TestCleanup]
        public void CleanUp()
        {
            LevelManager.Instance.Clear();
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

        [TestMethod]
        public void LevelManagerScoreRequiredForStarIs1000Test()
        {
            int expectedRequiredScore = 1000;
            m_level.SetStarRequirements(LevelStarId.Score, expectedRequiredScore);
            m_levelManager.AddZone(m_levelZone, LevelZoneId.Plain);
            m_levelManager.AddLevelToZone(LevelZoneId.Plain, LevelId.One, m_level);

            int actualRequiredScore = m_levelManager.GetStarRequirements(LevelZoneId.Plain, LevelId.One, LevelStarId.Score);
            Assert.AreEqual(expectedRequiredScore, actualRequiredScore);
        }

        [TestMethod]
        public void LevelManagerMolesWhackedRequiredForStarIs13Test()
        {
            int expectedRequiredMolesWhacked = 13;
            m_level.SetStarRequirements(LevelStarId.Hits, expectedRequiredMolesWhacked);
            m_levelManager.AddZone(m_levelZone, LevelZoneId.Plain);
            m_levelManager.AddLevelToZone(LevelZoneId.Plain, LevelId.One, m_level);

            int actualRequiredMolesWhacked = m_levelManager.GetStarRequirements(LevelZoneId.Plain, LevelId.One, LevelStarId.Hits);
            Assert.AreEqual(expectedRequiredMolesWhacked, actualRequiredMolesWhacked);
        }

        [TestMethod]
        public void LevelManagerWhackPercentRequiredForStarIs50Test()
        {
            int expectedRequiredWhackPercent = 50;
            m_level.SetStarRequirements(LevelStarId.HitPercent, expectedRequiredWhackPercent);
            m_levelZone.AddLevel(LevelId.One, m_level);
            m_levelManager.AddZone(m_levelZone, LevelZoneId.Plain);

            int actualRequiredWhackPercent = m_levelManager.GetStarRequirements(LevelZoneId.Plain, LevelId.One, LevelStarId.HitPercent);
            Assert.AreEqual(expectedRequiredWhackPercent, actualRequiredWhackPercent);
        }

        [TestMethod]
        public void LevelManagerScoreStarRequirementFailedTest()
        {
            int score = 1000;
            int requiredScore = 2000;
            m_level.SetStarRequirements(LevelStarId.Score, requiredScore);

            m_levelManager.AddZone(m_levelZone, LevelZoneId.Plain);
            m_levelManager.AddLevelToZone(LevelZoneId.Plain, LevelId.One, m_level);

            bool actualResults = m_levelManager.CheckStarRequirement(LevelZoneId.Plain, LevelId.One, LevelStarId.Score, score);
            Assert.IsFalse(actualResults);
        }
    }
}
