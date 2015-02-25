﻿//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace UnityVS.Striker.CSharp_Test.Score
{
    using System;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Score;

    [TestClass]
    public class ScoreTest
    {
        #region Test Members

        #endregion

        #region Test Methods

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestCleanup]
        public void CleanUp()
        {
        }

        [TestMethod]
        public void ScoreIncreaseScoreTo1Test()
        {
            int expectedScore = 1;
            int scoreAmount = 1;

            ScoreController score = new ScoreController();
            score.IncreaseScore(scoreAmount);

            int actualScore = score.Score;
            Assert.AreEqual(expectedScore, actualScore);
        }

        [TestMethod]
        public void ScoreWhackPercentage100PercentTest()
        {
            float expectedPercent = 100.0f;

            ScoreController score = new ScoreController();
            score.RecordWhackAttempt(true);

            float actualPercent = score.WhackPercentage;
            Assert.AreEqual(expectedPercent, actualPercent);
        }

        [TestMethod]
        public void ScoreWhackPercentage50PercentTest()
        {
            float expectedPercent = 50.0f;

            ScoreController score = new ScoreController();
            score.RecordWhackAttempt(true);
            score.RecordWhackAttempt(false);

            float actualPercent = score.WhackPercentage;
            Assert.AreEqual(expectedPercent, actualPercent);
        }

        [TestMethod]
        public void ScoreSuccessfulWhacksIs2Test()
        {
            int expectedWhacks = 2;

            ScoreController score = new ScoreController();
            score.RecordWhackAttempt(true);
            score.RecordWhackAttempt(true);

            int actualWhacks = score.Whacks;
            Assert.AreEqual(expectedWhacks, actualWhacks);
        }

        [TestMethod]
        public void ScoreWhackAttemptsIs5Test()
        {
            int expectedWhackAttempts = 5;

            ScoreController score = new ScoreController();
            score.RecordWhackAttempt(true);
            score.RecordWhackAttempt(true);
            score.RecordWhackAttempt(false);
            score.RecordWhackAttempt(false);
            score.RecordWhackAttempt(true);

            int actualWhackAttempts = score.WhackAttempts;
            Assert.AreEqual(expectedWhackAttempts, actualWhackAttempts);
        }

        #endregion
    }
}
