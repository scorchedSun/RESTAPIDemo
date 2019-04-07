using Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Builders
{
    public sealed class AddressBuilder : IAddressBuilder
    {
        private string zipCode;
        private string city;

        private AddressBuilder() {}

        public static IWithZipCodeAddressBuilder Create() => new AddressBuilder();

        public IAddress Build() => new Address(zipCode, city);

        public IAddressBuilder WithCity(string city)
        {
            this.city = city;
            return this;
        }

        public IWithCityAddressBuilder WithZipCode(string zipCode)
        {
            this.zipCode = zipCode;
            return this;
        }
    }
}
