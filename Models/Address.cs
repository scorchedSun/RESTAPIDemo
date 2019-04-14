using Contracts;

namespace Models
{
    /// <summary>
    /// Represents an address.
    /// </summary>
    public class Address : IAddress
    {
        public string ZipCode{ get; internal set; }

        public string City{ get; internal set; }

        /// <summary>
        /// Should never be instantiated directly. Use <see cref="AddressBuilder"/>
        /// </summary>
        internal Address() {}
    }
}
