using Contracts;
using Models.Builders;

namespace Models.Factories
{
    /// <summary>
    /// Responsible for creating new instances of <see cref="AddressBuilder"/>
    /// </summary>
    public class AddressBuilderFactory : IAddressBuilderFactory
    {
        /// <summary>
        /// Create a new <see cref="AddressBuilder"/>
        /// </summary>
        /// <returns>A <see cref="AddressBuilder"/></returns>
        public IAddressBuilder Create() => AddressBuilder.Create();
    }
}
