////////////////////////////////////////////////////////////////////////////////////////////
// <copyright file="Random.cs" company="Stephan Lemon">
// Copyright © 2010-2014 All rights reserved.
// </copyright>
// <author>Stephan Lemon</author>
////////////////////////////////////////////////////////////////////////////////////////////

namespace Assets.Scripts.Utilities.Random
{
    using System.Collections.Generic;

    public class RandomTester : IRandom
    {
        private int m_currentIndex;
        private List<int> m_randomNumbers = new List<int>();

        #region Constructors

        public RandomTester()
        {
        }

        #endregion

        #region IRandom Methods

        public int RandomInt(int maxVal)
        {
            return GetFakeRandomNumber();
        }

        public int RandomInt(int minVal, int maxVal)
        {
            return GetFakeRandomNumber();
        }

        public int RandomPercent()
        {
            return GetFakeRandomNumber();
        }

        public int RollDice(int dice, int sides)
        {
            return GetFakeRandomNumber();
        }

        public void SetTestNumbers(IList<int> nonRandomNumbers)
        {
            m_randomNumbers = new List<int>(nonRandomNumbers);
            m_currentIndex = 0;
        }

        #endregion

        private int GetFakeRandomNumber()
        {
            // If all numbers have been used then loop back to beginning
            if (m_currentIndex >= m_randomNumbers.Count)
                m_currentIndex = 0;

            int number = m_randomNumbers[m_currentIndex];
            ++m_currentIndex;

            return number;
        }
    }
}
