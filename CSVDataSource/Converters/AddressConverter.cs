using Contracts;
using Exceptions;
using System;

namespace CSVDataSource.Converters
{
    /// <summary>
    /// Converts a <see cref="string"/> to an <see cref="IAddress"/> and vice versa.
    /// </summary>
    public class AddressConverter : Utils.Converter<string, IAddress>
    {
        // Defines the number of parts a string representing an address is comprised of
        private const int NumberOfParts = 2;
        // Defines the separator between the different parts of the address
        private const char Separator = ' ';

        private readonly IAddressBuilderFactory addressBuilderFactory;

        /// <summary>
        /// Create a new <see cref="AddressConverter"/>.
        /// </summary>
        /// <param name="addressBuilderFactory">The factory used to create instances of <see cref="IAddressBuilder"/></param>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        public AddressConverter(IAddressBuilderFactory addressBuilderFactory)
        {
            if (addressBuilderFactory is null) throw new ArgumentNullException(nameof(addressBuilderFactory));
            this.addressBuilderFactory = addressBuilderFactory;
        }

        /// <summary>
        /// Convert a <see cref="string"/> to an <see cref="IAddress"/>.
        /// </summary>
        /// <param name="toConvert">The <see cref="string"/> to convert</param>
        /// <returns>The converted <see cref="string"/></returns>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        /// <exception cref="InvalidFormattedAddressException">If the value given by <paramref name="toConvert"/> isn't formatted correctly</exception>
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

        /// <summary>
        /// Convert an <see cref="IAddress"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="toConvert">The <see cref="IAddress"/> to convert</param>
        /// <returns>The converted <see cref="IAddress"/></returns>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        public override string Convert(IAddress toConvert)
        {
            if (toConvert is null) throw new ArgumentNullException(nameof(toConvert));

            return $"{toConvert.ZipCode} {toConvert.City}";
        }
    }
}
