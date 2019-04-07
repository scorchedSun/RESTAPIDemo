using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Exceptions
{
    public class AmbiguousIDException : Exception
    {
        public AmbiguousIDException() : base()
        {
        }

        public AmbiguousIDException(string message) : base(message)
        {
        }

        public AmbiguousIDException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AmbiguousIDException(int id)
            : base($"The ID '{id}' isn't unique")
        {}
    }
}
