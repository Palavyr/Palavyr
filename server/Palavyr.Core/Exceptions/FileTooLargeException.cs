#nullable enable
using System;

namespace Palavyr.Core.Exceptions
{
    public class FileTooLargeException : Exception
    {
        public FileTooLargeException()
        {
        }

        public FileTooLargeException(string? message) : base(message)
        {
        }
    }
}