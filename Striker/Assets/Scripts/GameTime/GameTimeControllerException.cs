//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.GameTime
{
    using System;
    using System.Runtime.Serialization;

    public class GameTimeControllerException : Exception
    {
        public GameTimeControllerException()
        {
        }

        public GameTimeControllerException(string message)
            : base(message)
        {
        }

        public GameTimeControllerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected GameTimeControllerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
