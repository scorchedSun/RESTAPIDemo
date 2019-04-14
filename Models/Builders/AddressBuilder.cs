using Contracts;
using System;

namespace Models.Builders
{
    /// <summary>
    /// Provides functionality to build new instances of <see cref="Address"/>.
    /// Each instance of <see cref="AddressBuilder"/> is one time use only, each
    /// subsequent call on <see cref="Build"/> returns null after the first.
    /// </summary>
    public sealed class AddressBuilder
        : IAddressBuilder
        , IAddressWithCityBuilder
        , IFinalAddressBuilder
    {
        private Address address;

        // Enforce usage of factory method to allow for returning the desired first interface
        private AddressBuilder() { address = new Address(); }

        /// <summary>
        /// Creates a new <see cref="AddressBuilder"/>
        /// </summary>
        /// <returns>A new <see cref="AddressBuilder"/> representing the first stage of the builder</returns>
        internal static IAddressBuilder Create() => new AddressBuilder();

        /// <summary>
        /// Creates a new <see cref="Address"/> using the values set in prior stages.
        /// </summary>
        /// <returns>An instance of a <see cref="Address"/> implementation on first call, <see cref="null"/> on further calls</returns>
        public IAddress Build()
        {
            var result = address;
            address = null;
            return result;
        }

        /// <summary>
        /// Set the <see cref="Address.City"/> property of the <see cref="Address"/>
        /// </summary>
        /// <param name="city">The city</param>
        /// <returns>Next stage of the builder</returns>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        /// <exception cref="ArgumentException">If <see cref="string.Empty"/> is passed</exception>
        public IFinalAddressBuilder WithCity(string city)
        {
            if (city is null) throw new ArgumentNullException(nameof(city));
            if (city.Equals(string.Empty)) throw new ArgumentException(nameof(city));
            address.City = city;
            return this;
        }

        /// <summary>
        /// Set the <see cref="Address.ZipCode"/> property of the <see cref="Address"/>
        /// </summary>
        /// <param name="zipCode">The zip code</param>
        /// <returns>Next stage of the builder</returns>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        /// <exception cref="ArgumentException">If <see cref="string.Empty"/> is passed</exception>
        public IAddressWithCityBuilder WithZipCode(string zipCode)
        {
            if (zipCode is null) throw new ArgumentNullException(nameof(zipCode));
            if (zipCode.Equals(string.Empty)) throw new ArgumentException(nameof(zipCode));
            address.ZipCode = zipCode;
            return this;
        }
    }
}
