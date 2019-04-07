using Contracts;
using System;

namespace Converters
{
    public class AddressConverter : Converter<string, IAddress>
    {
        private const int NumberOfParts = 2;

        private readonly IAdressWithZipCode addressBuilder;

        public AddressConverter(IAdressWithZipCode addressBuilder) => this.addressBuilder = addressBuilder;

        public override IAddress Convert(string toConvert)
        {
            if (toConvert is null) throw new ArgumentNullException(nameof(toConvert));

            string[] parts = toConvert.Split(new[] { ' ' }, NumberOfParts);
            if (parts.Length != NumberOfParts)
                    throw new FormatException($"The address provided '{toConvert}' isn't formatted correctly. Excpected 2 entries separated by an whitespace.");

            return addressBuilder
                .WithZipCode(parts[0])
                .WithCity(parts[1])
                .Build();
        }

        public override string Convert(IAddress toConvert)
        {
            if (toConvert is null) throw new ArgumentNullException(nameof(toConvert));

            return $"{toConvert.ZipCode} {toConvert.City}";
        }
    }
}
