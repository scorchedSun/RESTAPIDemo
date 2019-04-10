using System;
using System.Collections.Generic;
using System.Text;

namespace Exceptions
{
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
    }
}
