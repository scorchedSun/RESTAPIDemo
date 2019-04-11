using Contracts;
using Models.Builders;

namespace Models.Factories
{
    public class AddressBuilderFactory : IAddressBuilderFactory
    {
        public IAddressBuilder Create() => AddressBuilder.Create();
    }
}
