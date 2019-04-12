using Contracts;
using Exceptions;
using System;

namespace CSVDataSource.Converters
{
    public class AddressConverter : Utils.Converter<string, IAddress>
    {
        private const int NumberOfParts = 2;
        private const char Separator = ' ';

        private readonly IAddressBuilderFactory addressBuilderFactory;

        public AddressConverter(IAddressBuilderFactory addressBuilderFactory)
        {
            if (addressBuilderFactory is null) throw new ArgumentNullException(nameof(addressBuilderFactory));
            this.addressBuilderFactory = addressBuilderFactory;
        }

        public override IAddress Convert(string toConvert)
        {
            if (toConvert is null) throw new ArgumentNullException(nameof(toConvert));

            string[] parts = toConvert.Split(new[] { Separator }, NumberOfParts);
            if (parts.Length != NumberOfParts) throw new InvalidFormattedAddressException(toConvert, NumberOfParts, Separator);

            return addressBuilderFactory.Create()
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
