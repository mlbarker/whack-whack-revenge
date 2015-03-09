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

        ILevelZone m_levelZoneSubstitute;
        ILevelManager m_levelManagerSubstitute;
        ILevel m_levelSubstitute;

        #endregion

        [TestInitialize]
        public void InitializeTest()
        {
            m_levelZoneSubstitute = Substitute.For<ILevelZone>();
            m_levelManagerSubstitute = Substitute.For<ILevelManager>();
            m_levelSubstitute = Substitute.For<ILevel>();
        }

        [TestMethod]
        public void LevelZoneIdInLevelManagerTest()
        {
            m_levelManagerSubstitute.AddZone(m_levelZoneSubstitute, LevelZoneId.Plain);

            bool actualResult = m_levelManagerSubstitute.ContainsZone(LevelZoneId.Plain);
            Assert.IsTrue(actualResult);
        }
    }
}
