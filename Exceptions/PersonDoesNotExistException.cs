using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
    public class PersonDoesNotExistException : Exception
    {
        public PersonDoesNotExistException() : base()
        {
        }

        public PersonDoesNotExistException(string message) : base(message)
        {
        }

        public PersonDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PersonDoesNotExistException(int id)
            : base($"The person with the ID '{id}' doesn't exist")
        {}

        protected PersonDoesNotExistException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
