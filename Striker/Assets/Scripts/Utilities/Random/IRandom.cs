////////////////////////////////////////////////////////////////////////////////////////////
// <copyright file="IRandom.cs" company="Stephan Lemon">
// Copyright © 2010-2014 All rights reserved.
// </copyright>
// <author>Stephan Lemon</author>
////////////////////////////////////////////////////////////////////////////////////////////

namespace Assets.Scripts.Utilities.Random
{
    using System.Collections.Generic;

    public interface IRandom
    {

        int RandomInt(int maxValue);

        int RandomInt(int minValue, int maxValue);

        int RandomPercent();

        int RollDice(int dice, int sides);

        void SetTestNumbers(IList<int> nonRandomNumbers);
    }
}
