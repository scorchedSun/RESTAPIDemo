using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Utils.Exceptions
{
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
    }
}
