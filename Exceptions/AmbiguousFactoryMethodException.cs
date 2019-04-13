using System;
using System.Runtime.Serialization;

namespace Exceptions
{
    [Serializable]
    public class AmbiguousFactoryMethodException : Exception
    {
        public AmbiguousFactoryMethodException() : base()
        {
        }

        public AmbiguousFactoryMethodException(string message) : base(message)
        {
        }

        public AmbiguousFactoryMethodException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AmbiguousFactoryMethodException(Type type, string factoryMethodName)
            : base($"The type {type.FullName} defines multiple methods named '{factoryMethodName}'")
        {}

        protected AmbiguousFactoryMethodException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}
