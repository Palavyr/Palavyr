using System;

namespace Palavyr.Core.Exceptions
{
    public class MicroserviceException : Exception
    {
        public MicroserviceException()
        {
        }

        public MicroserviceException(string? message) : base(message)
        {
        }

        public MicroserviceException(string? message, Exception inner) : base(message, inner)
        {
            
        }
    }
}