using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
    public class UnsupportedColourException : Exception
    {
        public UnsupportedColourException() : base()
        {
        }

        public UnsupportedColourException(string message) : base(message)
        {
        }

        public UnsupportedColourException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UnsupportedColourException(Color colour)
            : base($"The colour '{colour.Name}' isn't supported")
        {
        }

        protected UnsupportedColourException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
