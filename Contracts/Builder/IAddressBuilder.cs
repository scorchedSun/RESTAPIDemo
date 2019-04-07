namespace Contracts
{
    public interface IAdressWithZipCode
    {
        IAddressWithCity WithZipCode(string zipCode);
    }

    public interface IAddressWithCity
    {
        IAddressBuilder WithCity(string city);
    }

    public interface IAddressBuilder
        : IAdressWithZipCode, IAddressWithCity
    {
        IAddress Build();
    }
}
