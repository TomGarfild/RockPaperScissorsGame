using System;

namespace Server.Exceptions
{
    public class MultiDeviceException : Exception
    {
        public MultiDeviceException()
        {
        }

        public MultiDeviceException(string message)
            : base(message)
        {
        }

        public MultiDeviceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
