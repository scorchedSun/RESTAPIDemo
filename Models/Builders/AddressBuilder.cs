using Contracts;

namespace Models.Builders
{
    public sealed class AddressBuilder
        : IAddressBuilder
        , IAddressWithCityBuilder
        , IFinalAddressBuilder
    {
        private string zipCode;
        private string city;

        private AddressBuilder() {}

        public static IAddressBuilder Create() => new AddressBuilder();

        public IAddress Build() => new Address(zipCode, city);

        public IFinalAddressBuilder WithCity(string city)
        {
            this.city = city;
            return this;
        }

        public IAddressWithCityBuilder WithZipCode(string zipCode)
        {
            this.zipCode = zipCode;
            return this;
        }
    }
}
