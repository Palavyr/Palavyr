#nullable enable
using System;

namespace Palavyr.Core.Exceptions
{
    public class DomainException : Exception
    {
        public string Message { get; set; } = "";
        public DomainException()
        {
        }

        public DomainException(string? message) : base(message)
        {
            Message = message;
        }
    }
}