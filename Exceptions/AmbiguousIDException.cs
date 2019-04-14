using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
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

        public AmbiguousIDException(uint id)
            : base($"The ID '{id}' isn't unique")
        {}

        protected AmbiguousIDException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
