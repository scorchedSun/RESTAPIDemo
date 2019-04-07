using CSVDataSource;
using System;

namespace RESTAPIDemo
{
    /// <summary>
    /// Converts the data source type as configured in appsettings.json to the actual representation.
    /// </summary>
    public class DataSourceTypeConverter : Converters.Converter<string, Type>
    {
        /// <summary>
        /// Converts the value from the appsettings.json file to the actual representation.
        /// </summary>
        /// <param name="toConvert">Value to convert</param>
        /// <returns>Matching <see cref="Type"/></returns>
        /// <exception cref="InvalidOperationException">Thrown if given value isn't recognized as an supported type.</exception>
        public override Type Convert(string toConvert)
        {
            switch (toConvert.ToLower())
            {
                case "csv":
                    return typeof(CSVPersonDataSource);
                default:
                    throw new InvalidOperationException($"Invalid configuration '{toConvert}'. Check your application settings.");
            }
        }

        // Not needed, therefore not implemented
        public override string Convert(Type toConvert) => throw new NotImplementedException();
    }
}
