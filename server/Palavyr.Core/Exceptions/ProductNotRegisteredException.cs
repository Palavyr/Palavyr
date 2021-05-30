using System;

namespace Palavyr.Core.Exceptions
{
    public class ProductNotRegisteredException : Exception
    {
        public ProductNotRegisteredException()
        {
        }

        public ProductNotRegisteredException(string? message) : base(message)
        {
        }
    }
}