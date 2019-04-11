using System;

namespace Exceptions
{
    public class InvalidFormattedAddressException : FormatException
    {
        public InvalidFormattedAddressException() : base()
        {
        }

        public InvalidFormattedAddressException(string message) : base(message)
        {
        }

        public InvalidFormattedAddressException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidFormattedAddressException(string address, int expectedParts, char separator)
            : base($"The address provided '{address}' isn't formatted correctly. Excpected {expectedParts} entries separated by '{separator}'.")
        {}
    }
}
