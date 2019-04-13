using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
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

        protected TooManyFieldsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
