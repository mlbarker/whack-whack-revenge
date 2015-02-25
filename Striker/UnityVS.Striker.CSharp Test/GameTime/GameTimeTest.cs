//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace UnityVS.Striker.CSharp_Test.GameTime
{
    using System;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Assets.Scripts.GameTime;
    using Assets.Scripts.Interfaces;

    [TestClass]
    public class GameTimeTest
    {
        #region Test Methods

        [TestMethod]
        public void GameTimeTimeElapsedTest()
        {
            int gameTimeAmount = 1;
            AutoResetEvent resetEvent = new AutoResetEvent(false);
            bool expectedTimeElapsed = true;
            GameTimeController.TimeElapsedEvent elapsedEvent = () => {};

            GameTimeController gameTimeController = new GameTimeController();
            gameTimeController.Initialize();
            gameTimeController.SetGameTime(gameTimeAmount);
            gameTimeController.SetTimeElapsedCallback(elapsedEvent);
            gameTimeController.Start();
            gameTimeController.UpdateTime();
            resetEvent.WaitOne(gameTimeAmount * 1000);
            gameTimeController.UpdateTime();

            bool actualTimeElapsed = gameTimeController.TimeElapsed;
            Assert.AreEqual(expectedTimeElapsed, actualTimeElapsed);
        }

        [TestMethod]
        public void GameTimeSetGameTimeTest()
        {
            int expectedTimeAmount = 1;

            GameTimeController gameTimeController = new GameTimeController();
            gameTimeController.Initialize();
            gameTimeController.SetGameTime(expectedTimeAmount);

            int actualTimeAmount = gameTimeController.GameTimeSeconds;
            Assert.AreEqual(expectedTimeAmount, actualTimeAmount);
        }

        [TestMethod]
        public void GameTimeCallbackIsNullTest()
        {
            int gameTimeAmount = 1;
            AutoResetEvent resetEvent = new AutoResetEvent(false);

            GameTimeController gameTimeController = new GameTimeController();
            gameTimeController.Initialize();
            gameTimeController.SetGameTime(gameTimeAmount);
            gameTimeController.Start();
            gameTimeController.UpdateTime();
            resetEvent.WaitOne(gameTimeAmount * 1000);

            try
            {
                gameTimeController.UpdateTime();
            }
            catch(GameTimeControllerException)
            {
            }
            catch(Exception)
            {
                Assert.Fail();
            }
        }

        #endregion
    }
}
