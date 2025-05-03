using System;

namespace Duo.Exceptions
{
    public class SectionServiceException : Exception
    {
        public SectionServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}