using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
    public class InvalidColourCodeException : Exception
    {
        public InvalidColourCodeException() : base()
        {
        }

        public InvalidColourCodeException(string code)
            : base($"The code '{code}' couldn't be matched to a colour")
        {
        }

        public InvalidColourCodeException(string code, Exception innerException) 
            : base($"The code '{code}' couldn't be matched to a colour", innerException)
        {
        }

        protected InvalidColourCodeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
