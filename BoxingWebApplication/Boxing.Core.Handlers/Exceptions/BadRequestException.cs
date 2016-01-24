using System;

namespace Boxing.Core.Handlers.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message = null)
            : base(message)
        { }
    }
}
