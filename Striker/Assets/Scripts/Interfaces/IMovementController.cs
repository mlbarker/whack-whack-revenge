//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IMovementController
    {
        void MoveIntoHole();
        void MoveOutOfHole();
        void MoveIntoHoleOnSwoon();
        void MoveToIdle();
        void MoveOnInjured();
    }
}
