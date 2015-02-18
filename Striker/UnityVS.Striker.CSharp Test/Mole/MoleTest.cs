//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace UnityVS.Striker.CSharp_Test.Mole
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Mole;
    using Assets.Scripts.Utilities.Random;
    using Assets.Scripts.Utilities.Timers;

    [TestClass]
    public class MoleTest
    {
        #region Test Members

        private IHealthController m_healthSubstitute;
        private IMovementController m_movementSubstitute;

        #endregion

        #region Test Methods

        [TestInitialize]
        public void Initialize()
        {
            m_healthSubstitute = Substitute.For<IHealthController>();
            m_movementSubstitute = Substitute.For<IMovementController>();
        }

        [TestCleanup]
        public void CleanUp()
        {
            m_healthSubstitute = null;
            m_movementSubstitute = null;
        }

        [TestMethod]
        public void MoleDecrementHealthBy1Test()
        {
            var mole = new MoleController();
            mole.health = 1;
            mole.SetHealthController(m_healthSubstitute);
            mole.SetMovementController(m_movementSubstitute);
            int amount = 1;
            int expectedHealth = 0;

            mole.Initialize();
            mole.DecrementHealth(amount);

            int actualHealth = mole.Health;
            Assert.AreEqual(expectedHealth, actualHealth);
        }

        [TestMethod]
        public void MoleStoppedMovingTest()
        {
            bool expectedIsMoving = false;

            var mole = new MoleController();
            mole.Initialize();
            mole.StoppedMoving();

            bool actualIsMoving = mole.IsMoving;
            Assert.AreEqual(expectedIsMoving, actualIsMoving);
        }

        [TestMethod]
        public void MoleIsMovingTest()
        {
            var mole = new MoleController();
            mole.health = 1;
            mole.maxSecondsDown = 1;
            mole.maxSecondsUp = 15;
            mole.SetHealthController(m_healthSubstitute);
            mole.SetMovementController(m_movementSubstitute);
            bool expectedIsMoving = true;
            AutoResetEvent resetEvent = new AutoResetEvent(false);
            int wait = 1000;

            mole.Initialize();
            resetEvent.WaitOne(wait);
            mole.Update();

            bool actualIsMoving = mole.IsMoving;
            Assert.AreEqual(expectedIsMoving, actualIsMoving);
        }

        [TestMethod]
        public void MoleIsRecoveringTest()
        {
            var mole = new MoleController();
            mole.health = 1;
            mole.maxSecondsDown = 15;
            mole.maxSecondsUp = 15;
            mole.SetHealthController(m_healthSubstitute);
            mole.SetMovementController(m_movementSubstitute);
            bool expectedRecoveryStatus = true;

            mole.Initialize();
            mole.Update();

            bool actualRecoveryStatus = mole.GetMoleStatus(MoleStatus.Recovering);
            Assert.AreEqual(expectedRecoveryStatus, actualRecoveryStatus);
        }

        [TestMethod]
        public void MoleIsInjuredTest()
        {
            var mole = new MoleController();
            mole.health = 1;
            mole.maxSecondsDown = 1;
            mole.maxSecondsUp = 15;
            int amount = 1;
            m_healthSubstitute.When(x => x.AdjustHealth()).Do(x => mole.DecrementHealth(amount));
            mole.SetHealthController(m_healthSubstitute);
            mole.SetMovementController(m_movementSubstitute);
            bool expectedInjuredStatus = true;
            AutoResetEvent resetEvent = new AutoResetEvent(false);
            int wait = 1800;

            mole.Initialize();
            mole.Update();
            resetEvent.WaitOne(wait);
            mole.Hit = true;
            mole.Update();

            bool actualInjuredStatus = mole.GetMoleStatus(MoleStatus.Injured);
            Assert.AreEqual(expectedInjuredStatus, actualInjuredStatus);
        }

        [TestMethod]
        public void MoleIsHealthyTest()
        {
            var mole = new MoleController();
            mole.health = 1;
            mole.maxSecondsDown = 15;
            mole.maxSecondsUp = 15;
            bool expectedHealthyStatus = true;


            mole.Initialize();
            mole.Update();


            bool actualHealthyStatus = mole.GetMoleStatus(MoleStatus.Healthy);
            Assert.AreEqual(expectedHealthyStatus, actualHealthyStatus);
        }

        [TestMethod]
        public void MoleGetHealthTickTest()
        {
            var mole = new MoleController();
            mole.health = 2;
            mole.maxSecondsDown = 1;
            mole.maxSecondsUp = 15;
            int amount = 1;
            m_healthSubstitute.When(x => x.AdjustHealth()).Do(x => mole.DecrementHealth(amount));
            m_healthSubstitute.When(x => x.RecoverHealth()).Do(x => mole.IncrementHealth(amount));
            mole.SetHealthController(m_healthSubstitute);
            mole.SetMovementController(m_movementSubstitute);
            AutoResetEvent resetEvent = new AutoResetEvent(false);
            int wait = 1200;
            int expectedHealth = 2;


            mole.Initialize();
            resetEvent.WaitOne(wait);
            mole.Update();
            mole.Hit = true;
            mole.Update();
            mole.Hit = true;
            mole.Update();
            resetEvent.WaitOne(wait);
            mole.Update();
            resetEvent.WaitOne(wait);
            mole.Update();


            int actualHealth = mole.Health;
            Assert.AreEqual(expectedHealth, actualHealth);
        }

        [TestMethod]
        public void MoleHealthControllerIsNullTest()
        {
            var mole = new MoleController();
            m_healthSubstitute = null;

            mole.SetHealthController(m_healthSubstitute);

            Assert.IsNull(m_healthSubstitute);
        }

        [TestMethod]
        public void MoleMovementControllerIsNullTest()
        {
            var mole = new MoleController();
            m_movementSubstitute = null;

            mole.SetMovementController(m_movementSubstitute);

            Assert.IsNull(m_movementSubstitute);
        }

        [TestMethod]
        public void MoleGetScoreValueTest()
        {
            int expectedScoreValue = 2;
            var mole = new MoleController();
            mole.health = 1;
            mole.maxSecondsDown = 15;
            mole.maxSecondsUp = 15;
            mole.scoreValue = 2;

            mole.Initialize();

            int actualScoreValue = mole.ScoreValue;
            Assert.AreEqual(expectedScoreValue, actualScoreValue);
        }

        #endregion

        #region Helper Methods
        #endregion
    }
}
