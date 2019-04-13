using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
    public class InvalidColourNameException : Exception
    {
        public InvalidColourNameException() : base()
        {
        }

        public InvalidColourNameException(string name) 
            : base($"The name '{name}' couldn't be matched to a colour")
        {
        }

        public InvalidColourNameException(string name, Exception innerException) 
            : base($"The name '{name}' couldn't be matched to a colour", innerException)
        {
        }

        protected InvalidColourNameException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
