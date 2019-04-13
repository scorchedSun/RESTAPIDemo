using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
    public class TooFewFieldsException : FormatException
    {
        public TooFewFieldsException() : base()
        {
        }

        public TooFewFieldsException(string message) : base(message)
        {
        }

        public TooFewFieldsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TooFewFieldsException(int numberOfFields, int expectedNumberOfFields)
            : base($"Too few fields. Expected {expectedNumberOfFields}, got {numberOfFields}.")
        {}

        protected TooFewFieldsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
