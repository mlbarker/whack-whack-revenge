//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace UnityVS.Striker.CSharp_Test
{
    using System;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Mole;

    [TestClass]
    public class MoleTest
    {
        [TestMethod]
        public void MoveOutOfHoleTimeElapsedTest()
        {
            AutoResetEvent resetEvent = new AutoResetEvent(false);
            bool actual;
            int health = 1;
            int holeTime = 1;
            var mole = GetMoleWithHoleTimes(health, false, holeTime, holeTime);
            var movement = GetMovementController();

            mole.SetMovementController(movement);

            mole.ClearReceivedCalls();
            mole.Initialize();
            mole.UpdateStatus();
            resetEvent.WaitOne(holeTime * 1000);
            mole.UpdateStatus();
            actual = mole.IsUp;

            movement.Received().MoveOutOfHole();
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void MoveIntoHoleTimeElapsedTest()
        {
            // setup
            AutoResetEvent resetEvent = new AutoResetEvent(false);
            bool actual;
            int health = 1;
            int holeTime = 1;
            var mole = GetMoleWithHoleTimes(health, true, holeTime, holeTime);
            var movement = GetMovementController();

            mole.SetMovementController(movement);

            // test
            mole.ClearReceivedCalls();
            mole.Initialize();
            mole.UpdateStatus();
            resetEvent.WaitOne(holeTime * 1000);
            mole.UpdateStatus();
            actual = mole.IsUp;

            // result
            movement.Received().MoveIntoHole();
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void MoveIntoHoleNoHealthAfterHitTest()
        {
            int hp = 1;
            int decrementAmount = 1;
            int holeTime = 5;
            var mole = GetMoleWithHoleTimes(hp, true, holeTime, holeTime);
            var movement = GetMovementController();
            var health = GetHealthController();

            mole.SetMovementController(movement);
            mole.SetHealthController(health);
            mole.Hit = true;
            health.When(x => x.AdjustHealth()).Do(x => mole.DecrementHealth(decrementAmount));

            // test
            mole.ClearReceivedCalls();
            mole.Initialize();
            mole.UpdateStatus();
            mole.UpdateStatus();

            // result
            int expectedHealth = hp - decrementAmount;
            int actualHealth = mole.Health;
            bool actual = mole.IsUp;

            health.Received().AdjustHealth();
            Assert.IsFalse(actual);
            Assert.AreEqual(expectedHealth, actualHealth);
        }

        [TestMethod]
        public void StayOutOfHoleWhenHitTest()
        {
            int hp = 5;
            int holeTime = 5;
            int decrementAmount = 1;
            var mole = GetMoleWithHoleTimes(hp, true, holeTime, holeTime);
            var movement = GetMovementController();
            var health = GetHealthController();

            mole.SetMovementController(movement);
            mole.SetHealthController(health);
            mole.Hit = true;
            health.When(x => x.AdjustHealth()).Do(x => mole.DecrementHealth(decrementAmount));

            // test
            mole.ClearReceivedCalls();
            mole.Initialize();
            mole.UpdateStatus();

            // result
            int expectedHp = hp - decrementAmount;
            int actualHp = mole.Health;
            bool actual = mole.IsUp;

            health.Received().AdjustHealth();
            Assert.IsTrue(actual);
            Assert.AreEqual(expectedHp, actualHp);
        }

        [TestMethod]
        public void MoleHealthIs4Test()
        {
            // setup
            int hp = 5;
            int holeTime = 5;
            int decrementAmount = 1;
            var mole = GetMoleWithHoleTimes(hp, true, holeTime, holeTime);
            var movement = GetMovementController();
            var health = GetHealthController();

            mole.SetMovementController(movement);
            mole.SetHealthController(health);
            mole.Hit = true;
            health.When(x => x.AdjustHealth()).Do(x => mole.DecrementHealth(decrementAmount));

            // test
            mole.ClearReceivedCalls();
            mole.Initialize();
            mole.UpdateStatus();

            // result
            int expectedHealth = hp - 1;
            int actualHealth = mole.Health;
            bool actual = mole.IsUp;

            health.Received().AdjustHealth();
            Assert.IsTrue(actual);
            Assert.AreEqual(expectedHealth, actualHealth);
        }

        private MoleController GetMoleWithNoHoleTimes(int health, bool moleIsUp)
        {
            var mole = Substitute.For<MoleController>();
            mole.health = health;
            SetHoleTimes(mole, 0, 0);

            if(moleIsUp)
            {
                mole.ToggleUp();
            }

            return mole;
        }

        private MoleController GetMoleWithHoleTimes(int health, bool moleIsUp, int inHoleTime, int outHoleTime)
        {
            var mole = Substitute.For<MoleController>();
            mole.health = health;
            SetHoleTimes(mole, inHoleTime, outHoleTime);

            if (moleIsUp)
            {
                mole.ToggleUp();
            }

            return mole;
        }

        private void SetHoleTimes(MoleController mole, int inHoleTime, int outHoleTime)
        {
            mole.maxSecondsInHole = inHoleTime;
            mole.maxSecondsOutOfHole = outHoleTime;
        }

        private IHealthController GetHealthController()
        {
            return Substitute.For<IHealthController>();
        }

        private IMovementController GetMovementController()
        {
            return Substitute.For<IMovementController>();
        }
    }
}
