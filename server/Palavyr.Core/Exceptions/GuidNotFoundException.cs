
using System;

namespace Palavyr.Core.Exceptions
{
    public class GuidNotFoundException : Exception
    {
        public GuidNotFoundException()
        {
        }

        public GuidNotFoundException(string? message) : base(message)
        {
        }
    }
}