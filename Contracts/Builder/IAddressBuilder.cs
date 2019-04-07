namespace Contracts
{
    public interface IAddressBuilder
    {
        IAddressWithCityBuilder WithZipCode(string zipCode);
    }

    public interface IAddressWithCityBuilder
    {
        IFinalAddressBuilder WithCity(string city);
    }

    public interface IFinalAddressBuilder
        : IAddressBuilder, IAddressWithCityBuilder
    {
        IAddress Build();
    }
}
