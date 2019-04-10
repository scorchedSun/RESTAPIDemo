using System;

namespace Exceptions
{
    public class NoFactoryMethodException : MissingMethodException
    {
        public NoFactoryMethodException() : base()
        {
        }

        public NoFactoryMethodException(string message) : base(message)
        {
        }

        public NoFactoryMethodException(string message, Exception inner) : base(message, inner)
        {
        }

        public NoFactoryMethodException(string className, string methodName) : base(className, methodName)
        {
        }
    }
}
