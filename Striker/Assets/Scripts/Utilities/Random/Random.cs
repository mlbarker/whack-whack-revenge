////////////////////////////////////////////////////////////////////////////////////////////
// <copyright file="Random.cs" company="Stephan Lemon">
// Copyright © 2010-2014 All rights reserved.
// </copyright>
// <author>Stephan Lemon</author>
////////////////////////////////////////////////////////////////////////////////////////////

namespace Assets.Scripts.Utilities.Random
{
    using System.Collections.Generic;

    /// <summary>
    /// A random number generator that allows various types of random
    /// number generation for the game.
    /// </summary>
    public class Random : IRandom
    {
        #region Member Variables

        /// <summary>
        /// The instance of the C# random number generator.
        /// </summary>
        private static System.Random m_random = new System.Random();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Random class.
        /// </summary>
        public Random()
        {
        }

        #endregion

        #region IRandom Methods

        /// <summary>
        /// Return a random number between 0 and maxVal (inclusive).
        /// </summary>
        /// <param name="maxVal">The maximum value that can be returned.</param>
        /// <returns>A random number between 0 and maxVal.</returns>
        public int RandomInt(int maxVal)
        {
            return m_random.Next(maxVal + 1);
        }

        /// <summary>
        /// Return a random number between minVal and maxVal (inclusive)
        /// </summary>
        /// <param name="minVal">The lowest value.</param>
        /// <param name="maxVal">The highest value.</param>
        /// <returns>A random value between minVal and maxVal inclusive.</returns>
        public int RandomInt(int minVal, int maxVal)
        {
            return m_random.Next(minVal, maxVal + 1);
        }

        /// <summary>
        /// Return a random percentage between 1 and 100.
        /// </summary>
        /// <returns>A random percent</returns>
        public int RandomPercent()
        {
            return m_random.Next(1, 101);
        }

        /// <summary>
        /// Return a dice roll (ex: 1d4, 2d6, etc)
        /// </summary>
        /// <param name="dice">The number of dice to roll.</param>
        /// <param name="sides">The sides on each dice.</param>
        /// <returns>The rolled value.</returns>
        public int RollDice(int dice, int sides)
        {
            int total = 0;

            for (int die = 0; die < dice; die++)
            {
                total += m_random.Next(1, sides + 1);
            }

            return total;
        }

        /// <summary>
        /// Set a sequence of non-random numbers to return from successive calls
        /// to the random number generator class.  This is for testing only.
        /// </summary>
        /// <param name="nonRandomNumbers">A list of non-random numbers to return in sequence.</param>
        public void SetTestNumbers(IList<int> nonRandomNumbers)
        {
        }

        #endregion
    }
}
