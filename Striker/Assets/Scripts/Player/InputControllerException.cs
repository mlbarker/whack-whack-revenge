//-----------------------------
// ImperfectlyCoded © 2014-2015
//-----------------------------

namespace Assets.Scripts.Player
{
    using System;
    using System.Runtime.Serialization;

    public class InputControllerException : Exception
    {
        public InputControllerException()
        {
        }

        public InputControllerException(string message)
            : base(message)
        {
        }

        public InputControllerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InputControllerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
