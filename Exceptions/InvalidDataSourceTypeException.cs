using System;

namespace Exceptions
{
    public class InvalidDataSourceTypeException : FormatException
    {
        public InvalidDataSourceTypeException()
        {
        }

        public InvalidDataSourceTypeException(string message) : base(message)
        {
        }

        public InvalidDataSourceTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidDataSourceTypeException(string configurationName, string configuredType)
            : base($"The data source configuration '{configurationName}' has an invalid type. The type '{configuredType}' is not supported.")
        {}

        public InvalidDataSourceTypeException(string configurationName, string configuredType, string expectedType)
            : base($"The data source configuration '{configurationName}' has an invalid type. Configured: {configuredType} Expected: {expectedType}")
        {}
    }
}
