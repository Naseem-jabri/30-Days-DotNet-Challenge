using System;

namespace AuthNest_API.Middleware
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }   
    }
}
