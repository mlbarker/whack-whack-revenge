//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Utilities.ServiceManager
{
    using System;

    public class ServiceException : Exception
    {
        public ServiceException(string message)
            :base(message)
        {
        }

        public ServiceException(string message, Exception inner)
            :base(message, inner)
        {
        }

        public ServiceException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            :base(info, context)
        {
        }
    }
}
