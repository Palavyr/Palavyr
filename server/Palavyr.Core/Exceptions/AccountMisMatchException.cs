using System;

namespace Palavyr.Core.Exceptions
{
    public class AccountMisMatchException : Exception
    {
        public string Message { get; set; } = "";

        public AccountMisMatchException()
        {
        }

        public AccountMisMatchException(string? message) : base(message)
        {
            Message = message;
        }
    }
}