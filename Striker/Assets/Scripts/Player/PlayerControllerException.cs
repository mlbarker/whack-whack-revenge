//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Player
{
    using System;
    using System.Runtime.Serialization;

    public class PlayerControllerException : Exception
    {
        public PlayerControllerException()
        {
        }

        public PlayerControllerException(string message)
            : base(message)
        {
        }

        public PlayerControllerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PlayerControllerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
