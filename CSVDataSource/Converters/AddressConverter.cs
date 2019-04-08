﻿using Contracts;
using System;

namespace CSVDataSource.Converters
{
    public class AddressConverter : Utils.Converter<string, IAddress>
    {
        private const int NumberOfParts = 2;

        private readonly IAddressBuilder addressBuilder;

        public AddressConverter(IAddressBuilder addressBuilder) => this.addressBuilder = addressBuilder;

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