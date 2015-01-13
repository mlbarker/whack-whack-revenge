//-----------------------------
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
        private IInputController m_inputSubstitute;

        [TestInitialize]
        public void Initialize()
        {
            m_inputSubstitute = Substitute.For<IInputController>();
            m_inputSubstitute.ClearReceivedCalls();
        }

        [TestCleanup]
        public void CleanUp()
        {
            m_inputSubstitute = null;
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
    }
}
