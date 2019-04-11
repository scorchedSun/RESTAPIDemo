using Contracts;
using System;

namespace Models.Builders
{
    public sealed class AddressBuilder
        : IAddressBuilder
        , IAddressWithCityBuilder
        , IFinalAddressBuilder
    {
        private Address address;

        private AddressBuilder() { address = new Address(); }

        public static IAddressBuilder Create() => new AddressBuilder();

        public IAddress Build()
        {
            var result = address;
            address = null;
            return result;
        }

        public IFinalAddressBuilder WithCity(string city)
        {
            if (city is null) throw new ArgumentNullException(nameof(city));
            if (city.Equals(string.Empty)) throw new ArgumentException(nameof(city));
            address.City = city;
            return this;
        }

        public IAddressWithCityBuilder WithZipCode(string zipCode)
        {
            if (zipCode is null) throw new ArgumentNullException(nameof(zipCode));
            if (zipCode.Equals(string.Empty)) throw new ArgumentException(nameof(zipCode));
            address.ZipCode = zipCode;
            return this;
        }
    }
}
