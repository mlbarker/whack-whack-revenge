//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Menu
{
    using System;
    using System.Runtime.Serialization;

    public class MenuNavigationControllerException : Exception
    {
        public MenuNavigationControllerException()
        {
        }

        public MenuNavigationControllerException(string message)
            : base(message)
        {
        }

        public MenuNavigationControllerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MenuNavigationControllerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
