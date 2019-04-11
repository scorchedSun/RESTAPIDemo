using System;
using System.Collections.Generic;
using System.Text;

namespace Exceptions
{
    public class TooManyFieldsException : FormatException
    {
        public TooManyFieldsException() : base()
        {
        }

        public TooManyFieldsException(string message) : base(message)
        {
        }

        public TooManyFieldsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TooManyFieldsException(int numberOfFields, int expectedNumberOfFields)
            : base($"Got too many fields. Expected {expectedNumberOfFields}, got {numberOfFields}.")
        {}
    }
}
