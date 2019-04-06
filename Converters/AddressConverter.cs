using Contracts;
using Models;
using System;

namespace Converters
{
    public class AddressConverter : Converter<string, IAddress>
    {
        private const int NumberOfParts = 2;

        public override IAddress Convert(string toConvert)
        {
            if (toConvert is null) throw new ArgumentNullException(nameof(toConvert));

            string[] parts = toConvert.Split(new[] { ' ' }, NumberOfParts);
            if (parts.Length != NumberOfParts)
                    throw new FormatException($"The address provided '{toConvert}' isn't formatted correctly. Excpected 2 entries separated by an whitespace.");

            return new Address(parts[0], parts[1]);
        }

        public override string Convert(IAddress toConvert)
        {
            if (toConvert is null) throw new ArgumentNullException(nameof(toConvert));

            return $"{toConvert.ZipCode} {toConvert.City}";
        }
    }
}
