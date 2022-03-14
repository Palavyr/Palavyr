using System;

namespace Palavyr.Core.Exceptions
{
    public class AccountMisMatchException : Exception
    {

        public AccountMisMatchException()
        {
        }

        public AccountMisMatchException(string? message) : base(message)
        {
        }
    }
}