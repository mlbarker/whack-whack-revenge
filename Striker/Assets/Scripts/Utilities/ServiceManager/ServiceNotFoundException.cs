//-----------------------------
// ImperfectlyCoded © 2014
//-----------------------------

namespace Assets.Scripts.Utilities.ServiceManager
{
    using System;

    [Serializable]
    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(string message)
            :base(message)
        {
        }

        public ServiceNotFoundException(string message, Exception inner)
            :base(message, inner)
        {
        }

        public ServiceNotFoundException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            :base(info, context)
        {
        }
    }
}
