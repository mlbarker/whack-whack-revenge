//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Formation
{
    using System;
    using UnityEngine;

    public class Position : MonoBehaviour
    {
        public Location location; 
    }

    public enum Location
    {
        Zero = 0,
        One,
        Two,
        Three,
        Four,
        Five,

        MAX_POSITIONS
    }
}
