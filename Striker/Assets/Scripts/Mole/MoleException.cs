//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Mole
{
    using System;
    using System.Runtime.Serialization;

    public class MoleException : Exception
    {
        public MoleException()
        {
        }

        public MoleException(string message)
            : base(message)
        {
        }

        public MoleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MoleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
