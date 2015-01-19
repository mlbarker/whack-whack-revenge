﻿//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace UnityVS.Striker.CSharp_Test.Player
{
    using System;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Mole;
    using Assets.Scripts.Player;
    using Assets.Scripts.Utilities.Timers;

    [TestClass]
    public class PlayerTest
    {
        private IHitController m_hitSubstitute;
        private IInputController m_inputSubstitute;

        [TestInitialize]
        public void Initialize()
        {
            m_inputSubstitute = Substitute.For<IInputController>();
            m_inputSubstitute.ClearReceivedCalls();

            m_hitSubstitute = Substitute.For<IHitController>();
            m_hitSubstitute.ClearReceivedCalls();
        }

        [TestCleanup]
        public void CleanUp()
        {
            m_inputSubstitute = null;
            m_hitSubstitute = null;
        }

        [TestMethod]
        public void PlayerAttackButtonPressedCanWhackTest()
        {
            var player = Substitute.For<PlayerController>();
            player.SetInputController(m_inputSubstitute);
            m_inputSubstitute.AttackButton().Returns(true);

            player.ClearReceivedCalls();
            player.Initialize();
            player.Update();

            m_inputSubstitute.Received().AttackButton();
            Assert.IsFalse(player.CanWhack);
            Assert.IsTrue(player.WhackTriggered);
            Assert.IsTrue(player.WhackCooldown);
        }

        [TestMethod]
        public void PlayerAttackButtonPressedCannotWhackTest()
        {
            var player = Substitute.For<PlayerController>();
            player.SetInputController(m_inputSubstitute);
            m_inputSubstitute.AttackButton().Returns(true);

            player.ClearReceivedCalls();
            player.Initialize();
            player.Update();
            player.Update();

            m_inputSubstitute.Received().AttackButton();
            Assert.IsFalse(player.CanWhack);
            Assert.IsTrue(player.WhackCooldown);
        }

        [TestMethod]
        public void PlayerAttackButtonPressedWhackCooldownElaspedTest()
        {
            int milliseconds = 1200;
            AutoResetEvent resetEvent = new AutoResetEvent(false);
            var player = Substitute.For<PlayerController>();
            player.SetInputController(m_inputSubstitute);
            player.buttonDelayMilliseconds = 1;
            m_inputSubstitute.AttackButton().Returns(true);

            player.ClearReceivedCalls();
            player.Initialize();
            player.Update();
            resetEvent.WaitOne(milliseconds);

            Assert.IsFalse(player.CanWhack);

            player.Update();

            Assert.IsTrue(player.CanWhack);
            Assert.IsFalse(player.WhackCooldown);
        }

        [TestMethod]
        public void PlayerSuccessfulHitTest()
        {
            var player = Substitute.For<PlayerController>();
            player.SetInputController(m_inputSubstitute);
            player.SetHitController(m_hitSubstitute);
            m_inputSubstitute.AttackButton().Returns(true);
            m_hitSubstitute.HitDetected().Returns(true);

            player.ClearReceivedCalls();
            player.Initialize();
            player.Update();

            m_inputSubstitute.Received().AttackButton();
            m_hitSubstitute.Received().HitDetected();
            Assert.IsTrue(player.WhackTriggered);
            Assert.IsTrue(player.MoleHit);
        }

        [TestMethod]
        public void PlayerUnsuccessfulHitTest()
        {
            var player = Substitute.For<PlayerController>();
            player.SetInputController(m_inputSubstitute);
            player.SetHitController(m_hitSubstitute);
            m_inputSubstitute.AttackButton().Returns(false);

            player.ClearReceivedCalls();
            player.Initialize();
            player.Update();

            m_inputSubstitute.Received().AttackButton();
            m_hitSubstitute.DidNotReceive().HitDetected();
            Assert.IsFalse(player.WhackTriggered);
            Assert.IsFalse(player.MoleHit);
        }
    }
}
