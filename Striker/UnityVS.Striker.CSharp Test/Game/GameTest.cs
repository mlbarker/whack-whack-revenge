﻿//-----------------------------
// ImperfectlyCoded © 2014-2015
//-----------------------------

namespace UnityVS.Striker.CSharp_Test.Game
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Assets.Scripts.Game;
    using Assets.Scripts.GameTime;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Mole;
    using Assets.Scripts.Player;
    using Assets.Scripts.Score;

    [TestClass]
    public class GameTest
    {
        #region Test Members

        private Dictionary<ExceptionType, Type> m_gameExceptions;

        private GameController m_gameController;
        private IGameTimeController m_gameTimeSubstitute;
        private IHealthController m_healthSubstitute;
        private IMovementController m_movementSubstitute;
        private IInputController m_inputSubstitute;
        private IScoreController m_scoreSubstitute;
        private IHitController m_hitSubstitute;
        private MoleController m_moleSubstitute;
        private PlayerController m_playerSubstitute;

        #endregion

        #region Test Methods

        [TestInitialize]
        public void Initialize()
        {
            m_gameController = new GameController();
            m_gameTimeSubstitute = Substitute.For<IGameTimeController>();
            m_healthSubstitute = Substitute.For<IHealthController>();
            m_movementSubstitute = Substitute.For<IMovementController>();
            m_inputSubstitute = Substitute.For<IInputController>();
            m_scoreSubstitute = Substitute.For<IScoreController>();
            m_hitSubstitute = Substitute.For<IHitController>();
            m_moleSubstitute = Substitute.For<MoleController>();
            m_playerSubstitute = Substitute.For<PlayerController>();

            m_gameExceptions = new Dictionary<ExceptionType, Type>()
            {
                { ExceptionType.GameTime, typeof(GameTimeControllerException) },
                { ExceptionType.Score, typeof(ScoreControllerException) },
                { ExceptionType.Player, typeof(PlayerControllerException) }
            };
        }

        [TestCleanup]
        public void CleanUp()
        {
            m_gameController = null;
            m_gameTimeSubstitute = null;
            m_healthSubstitute = null;
            m_movementSubstitute = null;
            m_inputSubstitute = null;
            m_scoreSubstitute = null;
            m_hitSubstitute = null;
            m_moleSubstitute = null;
            m_playerSubstitute = null;
        }

        [TestMethod]
        public void GameInitializeTest()
        {
            PrivateObject gameControllerAccessor = new PrivateObject(m_gameController);
            List<MoleController> moleControllers = new List<MoleController>();
            moleControllers.Add(new MoleController());
            string gameTimeControllerField = "m_gameTimeController";
            string scoreControllerField = "m_scoreController";
            string playerControllerField = "m_playerController";
            string moleControllersField = "m_moleControllers";
            gameControllerAccessor.SetField(gameTimeControllerField, new GameTimeController());
            gameControllerAccessor.SetField(scoreControllerField, new ScoreController());
            gameControllerAccessor.SetField(playerControllerField, new PlayerController());
            gameControllerAccessor.SetField(moleControllersField, moleControllers);

            m_gameController.Initialize();
            
            Assert.IsNotNull(gameControllerAccessor.GetField(gameTimeControllerField));
            Assert.IsNotNull(gameControllerAccessor.GetField(scoreControllerField));
            Assert.IsNotNull(gameControllerAccessor.GetField(playerControllerField));
            Assert.IsNotNull(gameControllerAccessor.GetField(moleControllersField));
        }

        [TestMethod]
        public void GameSetGameTimeTo60Test()
        {
            int expectedGameTime = 60;

            m_gameController.SetGameTime(expectedGameTime);

            int actualGameTime = m_gameController.GameTimeSeconds;
            Assert.AreEqual(expectedGameTime, actualGameTime);
        }

        [TestMethod]
        public void GameTimeUpCallbackIsNotNullTest()
        {
            PrivateObject gameControllerAccessor = new PrivateObject(m_gameController);
            string gameTimeControllerField = "m_gameTimeController";
            GameTimeController.TimeElapsedEvent expectedElapsedEvent = new GameTimeController.TimeElapsedEvent(GameEndCallback);

            m_gameController.SetOnGameEndCallback(GameEndCallback);
            GameTimeController gameTimeController = (GameTimeController)gameControllerAccessor.GetField(gameTimeControllerField);

            GameTimeController.TimeElapsedEvent actualElapsedEvent = (GameTimeController.TimeElapsedEvent)gameTimeController.TimeUpCallback;
            Assert.IsNotNull(actualElapsedEvent);
        }

        //[TestMethod]
        //public void GameTimeUpTest()
        //{
        //    int waitTime = 2;
        //    int actualTimeLeft = 0;
        //    bool attackButtonHit = true;
        //    int health = 1;
        //    int upTime = 1;
        //    int downTime = 1;
        //    var game = SetUpGame(waitTime, attackButtonHit, health, upTime, downTime);

        //    m_gameTimeSubstitute.GameTimeSeconds.Returns(x => game.gameTimeSeconds);
        //    m_gameTimeSubstitute.GameTimeSecondsLeft.Returns(x => --waitTime);

        //    game.ClearReceivedCalls();
        //    game.Initialize();
        //    game.Update();
        //    game.Update();

        //    actualTimeLeft = waitTime;

        //    m_gameTimeSubstitute.Received().SetGameTime(Arg.Any<int>());
        //    m_gameTimeSubstitute.Received().UpdateTime();
        //    m_gameTimeSubstitute.Received().TimeUpCallback();
        //    Assert.AreEqual(0, actualTimeLeft);
        //}

        //[TestMethod]
        //public void GameSuccessfulMoleHitTest()
        //{
        //    bool attackButtonWasHit = true;
        //    int gameTimeSeconds = 1;
        //    int health = 1;
        //    int upTime = 120;
        //    int downTime = 120;
        //    var game = SetUpGame(gameTimeSeconds, attackButtonWasHit, health, upTime, downTime);
        //    m_hitSubstitute.HitDetected().Returns(true);
        //    m_hitSubstitute.When(x => x.HitDetected()).Do(x => m_moleSubstitute.Hit = true);

        //    game.ClearReceivedCalls();
        //    game.Initialize();
        //    game.Update();
        //    game.Update();

        //    m_gameTimeSubstitute.DidNotReceive().TimeUpCallback();
        //    m_inputSubstitute.Received().AttackButton();
        //    int expectedHealth = 0;
        //    int actualHealth = m_moleSubstitute.Health;
        //    m_movementSubstitute.Received().MoveIntoHole();
        //    Assert.IsFalse(m_moleSubstitute.IsUp);
        //    Assert.IsFalse(game.IsTimeUp);
        //    Assert.AreEqual(expectedHealth, actualHealth);
        //}

        //[TestMethod]
        //public void GameUnsuccessfulMoleHitTest()
        //{
        //    bool attackButtonWasHit = true;
        //    int gameTimeSeconds = 1;
        //    int health = 1;
        //    int upTime = 10;
        //    int downTime = 10;
        //    var game = SetUpGame(gameTimeSeconds, attackButtonWasHit, health, upTime, downTime);

        //    game.ClearReceivedCalls();
        //    game.Initialize();
        //    game.Update();

        //    m_gameTimeSubstitute.DidNotReceive().TimeUpCallback();
        //    m_movementSubstitute.DidNotReceive().MoveIntoHole();
        //    m_movementSubstitute.DidNotReceive().MoveOutOfHole();
        //    m_inputSubstitute.Received().AttackButton();
        //    Assert.IsFalse(m_moleSubstitute.IsUp);
        //    Assert.IsFalse(game.IsTimeUp);
        //}

        //[TestMethod]
        //public void GameScoreOnSuccesfulHitTest()
        //{
        //    bool attackButtonWasHit = true;
        //    int gameTimeSeconds = 99;
        //    int health = 1;
        //    int upTime = 120;
        //    int downTime = 120;
        //    int score = 0;
        //    var game = SetUpGame(gameTimeSeconds, attackButtonWasHit, health, upTime, downTime);
        //    m_moleSubstitute.Hit = true;
        //    //m_scoreSubstitute.When(x => x.ScoreUpdate()).Do(x => ++score);

        //    game.ClearReceivedCalls();
        //    game.Initialize();
        //    game.Update();
        //    game.Update();

        //    //m_scoreSubstitute.Received().ScoreUpdate();
        //    //m_scoreSubstitute.Received().HitPercentageUpdate();
        //    int expectedScore = 1;
        //    int actualScore = score;
        //    Assert.AreEqual(expectedScore, actualScore);
        //    int expectedHealth = 0;
        //    int actualHealth = m_moleSubstitute.Health;
        //    Assert.AreEqual(expectedHealth, actualHealth);
        //}

        //[TestMethod]
        //public void GameNoScoreOnUnsuccesfulHitTest()
        //{
        //    bool attackButtonWasHit = true;
        //    int gameTimeSeconds = 99;
        //    int health = 1;
        //    int upTime = 120;
        //    int downTime = 120;
        //    int score = 0;
        //    var game = SetUpGame(gameTimeSeconds, attackButtonWasHit, health, upTime, downTime);
        //    //m_scoreSubstitute.When(x => x.ScoreUpdate()).Do(x => ++score);

        //    game.ClearReceivedCalls();
        //    game.Initialize();
        //    game.Update();
        //    game.Update();

        //    //m_scoreSubstitute.DidNotReceive().ScoreUpdate();
        //    //m_scoreSubstitute.Received().HitPercentageUpdate();
        //    int expectedScore = 0;
        //    int actualScore = score;
        //    Assert.AreEqual(expectedScore, actualScore);
        //    int expectedHealth = 1;
        //    int actualHealth = m_moleSubstitute.Health;
        //    Assert.AreEqual(expectedHealth, actualHealth);
        //}

        //[TestMethod]
        //public void GameScore100HitPercentageOnSuccesfulHitTest()
        //{
        //    bool attackButtonWasHit = true;
        //    int gameTimeSeconds = 99;
        //    int health = 1;
        //    int upTime = 120;
        //    int downTime = 120;
        //    int score = 0;
        //    float actualHitPercentage = 0.0f;
        //    int attempts = 0;

        //    var game = SetUpGame(gameTimeSeconds, attackButtonWasHit, health, upTime, downTime);
        //    m_moleSubstitute.Hit = true;
        //    m_hitSubstitute.HitDetected().Returns(true);
        //    //m_scoreSubstitute.When(x => x.ScoreUpdate()).Do(x => ++score);
        //    //m_scoreSubstitute.When(x => x.HitPercentageUpdate()).Do(x => ++attempts);

        //    game.ClearReceivedCalls();
        //    game.Initialize();
        //    game.Update();
        //    game.Update();

        //    //m_scoreSubstitute.Received().ScoreUpdate();
        //    //m_scoreSubstitute.Received().HitPercentageUpdate();
        //    int expectedScore = 1;
        //    int actualScore = score;
        //    Assert.AreEqual(expectedScore, actualScore);

        //    float expectedHitPercentage = 1.0f;
        //    actualHitPercentage = score / attempts;
        //    Assert.AreEqual(expectedHitPercentage, actualHitPercentage);

        //    int expectedHealth = 0;
        //    int actualHealth = m_moleSubstitute.Health;
        //    Assert.AreEqual(expectedHealth, actualHealth);
        //}

        #endregion

        #region Helper Methods

        private void GameEndCallback()
        {
        }

        //private GameController SetUpGame(int gameTimeSeconds, bool attackButtonHit, int health, int upTimeSeconds, int downTimeSeconds)
        //{
        //    GameController game = Substitute.For<GameController>();
        //    m_gameTimeSubstitute.GameTimeSecondsLeft.Returns(gameTimeSeconds);
        //    game.SetGameTimeController(m_gameTimeSubstitute);
        //    game.SetScoreController(m_scoreSubstitute);

        //    SetUpPlayer(attackButtonHit);
        //    game.SetPlayerController(m_playerSubstitute);

        //    SetUpMole(health, downTimeSeconds, upTimeSeconds);
        //    game.SetMoleController(m_moleSubstitute);

        //    return game;
        //}

        //private void SetUpPlayer(bool attackButtonHit)
        //{
        //    m_inputSubstitute.AttackButton().Returns(attackButtonHit);
        //    m_playerSubstitute.SetInputController(m_inputSubstitute);
        //    m_playerSubstitute.SetHitController(m_hitSubstitute);
        //}

        //private void SetUpMole(int health, int downTime, int upTime)
        //{
        //    m_moleSubstitute.health = health;
        //    m_moleSubstitute.maxSecondsDown = downTime;
        //    m_moleSubstitute.maxSecondsUp = upTime;

        //    m_healthSubstitute.When(x => x.AdjustHealth()).Do(x => m_moleSubstitute.DecrementHealth(1));

        //    m_moleSubstitute.SetHealthController(m_healthSubstitute);
        //    m_moleSubstitute.SetMovementController(m_movementSubstitute);
        //}

        #endregion

        #region Helper Enums

        public enum ExceptionType
        {
            GameTime,
            Score,
            Player
        }

        #endregion
    }
}
