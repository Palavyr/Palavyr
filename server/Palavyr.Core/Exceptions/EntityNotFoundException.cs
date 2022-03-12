using System;

namespace Palavyr.Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public string Message { get; set; } = "";

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string? message) : base(message)
        {
            Message = message;
        }
    }
}