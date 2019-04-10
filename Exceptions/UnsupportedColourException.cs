using System;
using System.Drawing;

namespace Utils.Exceptions
{
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
    }
}
