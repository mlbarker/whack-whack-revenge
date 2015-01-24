//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Player
{
    using System;
    using System.Runtime.Serialization;

    public class HitControllerException : Exception
    {
        public HitControllerException()
        {
        }

        public HitControllerException(string message)
            : base(message)
        {
        }

        public HitControllerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected HitControllerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
