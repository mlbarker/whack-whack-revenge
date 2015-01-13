//-----------------------------
// ImperfectlyCoded © 2014-2015
//-----------------------------

namespace Assets.Scripts.Utilities.Timers
{
    using System;
    using System.Runtime.Serialization;

    public class TimerException : Exception
    {
        public TimerException()
        {
        }

        public TimerException(string message)
            : base(message)
        {
        }

        public TimerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected TimerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
