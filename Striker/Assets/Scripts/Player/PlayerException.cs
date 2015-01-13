//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Player
{
    using System;
    using System.Runtime.Serialization;

    public class PlayerException : Exception
    {
        public PlayerException()
        {
        }

        public PlayerException(string message)
            : base(message)
        {
        }

        public PlayerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PlayerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
