namespace Contracts
{
    public interface IAddressWithZipCodeBuilder
    {
        IAddressWithCityBuilder WithZipCode(string zipCode);
    }

    public interface IAddressWithCityBuilder
    {
        IAddressBuilder WithCity(string city);
    }

    public interface IAddressBuilder
        : IAddressWithZipCodeBuilder, IAddressWithCityBuilder
    {
        IAddress Build();
    }
}
