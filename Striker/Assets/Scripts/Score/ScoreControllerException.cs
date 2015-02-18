//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Score
{
    using System;
    using System.Runtime.Serialization;

    public class ScoreControllerException : Exception
    {
        public ScoreControllerException()
        {
        }

        public ScoreControllerException(string message)
            : base(message)
        {
        }

        public ScoreControllerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ScoreControllerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
