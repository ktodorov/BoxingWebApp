using System;

namespace Boxing.Core.Handlers.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = null)
            : base(message)
        { }
    }
}
