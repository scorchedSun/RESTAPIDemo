using System;

namespace Utils.Exceptions
{
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
    }
}
