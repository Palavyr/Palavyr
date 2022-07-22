
using System;

namespace Palavyr.Core.Exceptions
{
    public class PalavyrStartupException : Exception
    {
        public PalavyrStartupException()
        {
        }

        public PalavyrStartupException(string? message) : base(message)
        {
            
        }
    }
}