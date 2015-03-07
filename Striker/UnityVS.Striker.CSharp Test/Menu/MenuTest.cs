//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace UnityVS.Striker.CSharp_Test.Menu
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Assets.Scripts.Interfaces;
    using Assets.Scripts.Menu;

    [TestClass]
    public class MenuTest
    {
        #region Test Members

        IMenuNavigationController m_subMenuNavigation;

        #endregion

        #region Unit Test Methods

        [TestInitialize]
        public void Initialize()
        {
            m_subMenuNavigation = Substitute.For<IMenuNavigationController>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            m_subMenuNavigation = null;
        }

        [TestMethod]
        public void MenuMenuNavigationControllerNotNullTest()
        {
            MenuController testMenuController = new MenuController();

            testMenuController.SetMenuNavigationController(m_subMenuNavigation);
            testMenuController.Initialize();

            MenuState result = testMenuController.CurrentMenuState;
            Assert.IsNotNull(m_subMenuNavigation);
        }

        [TestMethod]
        public void MenuThrowMenuNavigationControllerExceptionTest()
        {
            Exception caughtException = null;
            m_subMenuNavigation = null;
            MenuController testMenuController = new MenuController();

            testMenuController.Initialize();
            caughtException = ExceptionTest(testMenuController);
            
            Assert.IsNotNull(caughtException);
            Assert.IsInstanceOfType(caughtException, typeof(MenuNavigationControllerException));
        }

        [TestMethod]
        public void MenuPlayGameSelectedTest()
        {
            MenuState expectedResult = MenuState.GameStart;
            MenuController testMenuController = new MenuController();
            testMenuController.SetMenuNavigationController(m_subMenuNavigation);

            testMenuController.Initialize();
            testMenuController.StartGame();

            MenuState result = testMenuController.CurrentMenuState;
            m_subMenuNavigation.Received().RunStartGame();
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void MenuOptionsSelectedTest()
        {
            MenuState expectedResult = MenuState.Options;
            MenuController testMenuController = new MenuController();
            testMenuController.SetMenuNavigationController(m_subMenuNavigation);

            testMenuController.Initialize();
            testMenuController.OptionsMenu();

            MenuState result = testMenuController.CurrentMenuState;
            m_subMenuNavigation.Received().RunOptions();
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void MenuStatsSelectedTest()
        {
            MenuState expectedResult = MenuState.Stats;
            MenuController testMenuController = new MenuController();
            testMenuController.SetMenuNavigationController(m_subMenuNavigation);

            testMenuController.Initialize();
            testMenuController.StatsMenu();

            MenuState result = testMenuController.CurrentMenuState;
            m_subMenuNavigation.Received().RunStats();
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void MenuExitSelectedTest()
        {
            MenuState expectedResult = MenuState.Exit;
            MenuController testMenuController = new MenuController();
            testMenuController.SetMenuNavigationController(m_subMenuNavigation);

            testMenuController.Initialize();
            testMenuController.ExitApp();

            MenuState result = testMenuController.CurrentMenuState;
            m_subMenuNavigation.Received().RunExit();
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void MenuBackSelectedWhenInStatsMenuTest()
        {
            MenuState expectedResult = MenuState.Main;
            MenuController testMenuController = new MenuController();
            testMenuController.SetMenuNavigationController(m_subMenuNavigation);

            testMenuController.Initialize();
            testMenuController.StatsMenu();
            testMenuController.BackToPreviousMenu();

            MenuState result = testMenuController.CurrentMenuState;
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void MenuCheckIfCurrentMenuStateIsStatsMenu()
        {
            MenuController testMenuController = new MenuController();
            testMenuController.SetMenuNavigationController(m_subMenuNavigation);

            testMenuController.Initialize();
            testMenuController.StatsMenu();
            testMenuController.BackToPreviousMenu();
            testMenuController.StatsMenu();

            bool result = testMenuController.CheckCurrentMenuState(MenuState.Stats);
            Assert.IsTrue(result);
        }

        #endregion

        #region Helper Methods

        private Exception ExceptionTest(MenuController menuController)
        {
            try
            {
                menuController.SetMenuNavigationController(m_subMenuNavigation);
            }
            catch (Exception ex)
            {
                return ex;
            }

            return null;
        }

        #endregion
    }
}
