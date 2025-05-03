// File: Duo/Exceptions/SectionServiceProxyException.cs
using System;

namespace Duo.Exceptions
{
    public class SectionServiceProxyException : Exception
    {
        public SectionServiceProxyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
