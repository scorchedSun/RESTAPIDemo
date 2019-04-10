using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Exceptions
{
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
    }
}
