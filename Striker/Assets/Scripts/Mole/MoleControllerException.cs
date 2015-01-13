//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Mole
{
    using System;
    using System.Runtime.Serialization;

    public class MoleControllerException : Exception
    {
        public MoleControllerException()
        {
        }

        public MoleControllerException(string message)
            : base(message)
        {
        }

        public MoleControllerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MoleControllerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
