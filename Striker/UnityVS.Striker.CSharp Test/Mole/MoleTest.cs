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

        //[TestMethod]
        //public void MoleMoveOutOfHoleTimeElapsedTest()
        //{
        //    AutoResetEvent resetEvent = new AutoResetEvent(false);
        //    bool actual;
        //    int health = 1;
        //    int holeTime = 1;
        //    var mole = GetMoleWithHoleTimes(health, false, holeTime, holeTime * 120);
        //    RandomTester randomTester = new RandomTester();
        //    randomTester.SetTestNumbers(new List<int> { 1, 1 });
        //    mole.SetRandomObject(randomTester);
        //    mole.SetMovementController(m_movementSubstitute);

        //    mole.ClearReceivedCalls();
        //    mole.Initialize();
        //    mole.UpdateStatus();
        //    resetEvent.WaitOne(holeTime * 1500);
        //    mole.UpdateStatus();
        //    actual = mole.IsUp;

        //    m_movementSubstitute.Received().MoveOutOfHole();
        //    Assert.IsTrue(actual);
        //}

        //[TestMethod]
        //public void MoleMoveIntoHoleTimeElapsedTest()
        //{
        //    // setup
        //    AutoResetEvent resetEvent = new AutoResetEvent(false);
        //    bool actual;
        //    int health = 1;
        //    int holeTime = 1;
        //    var mole = GetMoleWithHoleTimes(health, true, holeTime * 120, holeTime);
        //    mole.SetMovementController(m_movementSubstitute);

        //    // test
        //    mole.ClearReceivedCalls();
        //    mole.Initialize();
        //    mole.UpdateStatus();
        //    resetEvent.WaitOne(holeTime * 1500);
        //    mole.UpdateStatus();
        //    actual = mole.IsUp;

        //    // result
        //    m_movementSubstitute.Received().MoveIntoHole();
        //    Assert.IsFalse(actual);
        //}

        //[TestMethod]
        //public void MoleMoveIntoHoleNoHealthAfterHitTest()
        //{
        //    int hp = 1;
        //    int decrementAmount = 1;
        //    int holeTime = 5;
        //    var mole = GetMoleWithHoleTimes(hp, true, holeTime, holeTime);

        //    mole.SetMovementController(m_movementSubstitute);
        //    mole.SetHealthController(m_healthSubstitute);
        //    mole.Hit = true;
        //    m_healthSubstitute.When(x => x.AdjustHealth()).Do(x => mole.DecrementHealth(decrementAmount));

        //    // test
        //    mole.ClearReceivedCalls();
        //    mole.Initialize();
        //    mole.UpdateStatus();
        //    mole.UpdateStatus();

        //    // result
        //    int expectedHealth = hp - decrementAmount;
        //    int actualHealth = mole.Health;
        //    bool actual = mole.IsUp;

        //    m_healthSubstitute.Received().AdjustHealth();
        //    Assert.IsFalse(actual);
        //    Assert.AreEqual(expectedHealth, actualHealth);
        //}

        //[TestMethod]
        //public void MoleStayOutOfHoleWhenHitTest()
        //{
        //    int hp = 5;
        //    int holeTime = 5;
        //    int decrementAmount = 1;
        //    var mole = GetMoleWithHoleTimes(hp, true, holeTime, holeTime);
        //    mole.SetMovementController(m_movementSubstitute);
        //    mole.SetHealthController(m_healthSubstitute);
        //    mole.Hit = true;
        //    m_healthSubstitute.When(x => x.AdjustHealth()).Do(x => mole.DecrementHealth(decrementAmount));

        //    // test
        //    mole.ClearReceivedCalls();
        //    mole.Initialize();
        //    mole.UpdateStatus();

        //    // result
        //    int expectedHp = hp - decrementAmount;
        //    int actualHp = mole.Health;
        //    bool actual = mole.IsUp;

        //    m_healthSubstitute.Received().AdjustHealth();
        //    Assert.IsTrue(actual);
        //    Assert.AreEqual(expectedHp, actualHp);
        //}

        //[TestMethod]
        //public void MoleHealthIs4Test()
        //{
        //    // setup
        //    int hp = 5;
        //    int holeTime = 5;
        //    int decrementAmount = 1;
        //    var mole = GetMoleWithHoleTimes(hp, true, holeTime, holeTime);
        //    mole.SetMovementController(m_movementSubstitute);
        //    mole.SetHealthController(m_healthSubstitute);
        //    mole.Hit = true;
        //    m_healthSubstitute.When(x => x.AdjustHealth()).Do(x => mole.DecrementHealth(decrementAmount));

        //    // test
        //    mole.ClearReceivedCalls();
        //    mole.Initialize();
        //    mole.UpdateStatus();

        //    // result
        //    int expectedHealth = hp - 1;
        //    int actualHealth = mole.Health;
        //    bool actual = mole.IsUp;

        //    m_healthSubstitute.Received().AdjustHealth();
        //    Assert.IsTrue(actual);
        //    Assert.AreEqual(expectedHealth, actualHealth);
        //}

        //[TestMethod]
        //public void MoleUpTimerStartTest()
        //{
        //    // setup
        //    int hp = 5;
        //    int holeTime = 1;
        //    bool upTime = true;
        //    var mole = GetMoleWithHoleTimes(hp, upTime, holeTime * 100, holeTime);
        //    mole.SetMovementController(m_movementSubstitute);
        //    mole.SetHealthController(m_healthSubstitute);

        //    AutoResetEvent resetEvent = new AutoResetEvent(false);
        //    mole.StartMoleTimer(upTime);
        //    resetEvent.WaitOne(2000);
        //    mole.UpdateStatus();

        //    Assert.IsFalse(mole.IsUp);
        //}

        #endregion

        #region Helper Methods

        private MoleController GetMoleWithNoHoleTimes(int health)
        {
            var mole = Substitute.For<MoleController>();
            mole.health = health;
            SetHoleTimes(mole, 0, 0);

            return mole;
        }

        private MoleController GetMoleWithHoleTimes(int health, int downTime, int upTime)
        {
            var mole = Substitute.For<MoleController>();
            mole.health = health;
            SetHoleTimes(mole, downTime, upTime);

            return mole;
        }

        private void SetHoleTimes(MoleController mole, int downTime, int upTime)
        {
            mole.maxSecondsDown = downTime;
            mole.maxSecondsUp = upTime;
        }

        #endregion
    }
}
