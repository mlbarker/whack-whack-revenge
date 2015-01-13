//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Game
{
    using System;
    using System.Runtime.Serialization;

    public class GameControllerException : Exception
    {
        public GameControllerException()
        {
        }

        public GameControllerException(string message)
            : base(message)
        {
        }

        public GameControllerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected GameControllerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
