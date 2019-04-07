using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IWithZipCodeAddressBuilder
    {
        IWithCityAddressBuilder WithZipCode(string zipCode);
    }

    public interface IWithCityAddressBuilder
    {
        IAddressBuilder WithCity(string city);
    }

    public interface IAddressBuilder
        : IWithZipCodeAddressBuilder, IWithCityAddressBuilder
    {
        IAddress Build();
    }
}
