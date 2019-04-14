namespace Contracts
{
    /*
     * This collection of interfaces defines the functionality of a
     * builder for IAddress. By using separate interfaces and returning
     * another interface per stage the user is forced to call all 
     * necesseary setters prior to calling the Build() method.
     */

    /// <summary>
    /// Entry point for building an <see cref="IAddress"/>.
    /// </summary>
    public interface IAddressBuilder
    {
        /// <summary>
        /// Set the <see cref="IAddress.ZipCode"/> property of the <see cref="IAddress"/>
        /// </summary>
        /// <param name="zipCode">The zip code</param>
        /// <returns>Next stage of the builder</returns>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        /// <exception cref="ArgumentException">If <see cref="string.Empty"/> is passed</exception>
        IAddressWithCityBuilder WithZipCode(string zipCode);
    }

    /// <summary>
    /// Second stage for building an <see cref="IAddress"/>
    /// </summary>
    public interface IAddressWithCityBuilder
    {
        /// <summary>
        /// Set the <see cref="IAddress.City"/> property of the <see cref="IAddress"/>
        /// </summary>
        /// <param name="city">The city</param>
        /// <returns>Next stage of the builder</returns>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        /// <exception cref="ArgumentException">If <see cref="string.Empty"/> is passed</exception>
        IFinalAddressBuilder WithCity(string city);
    }

    /// <summary>
    /// Last stage for building an <see cref="IAddress"/>
    /// </summary>
    public interface IFinalAddressBuilder
    {
        /// <summary>
        /// Creates a new <see cref="IAddress"/> using the values set in prior stages.
        /// </summary>
        /// <returns>An instance of a <see cref="IAddress"/> implementation on first call, <see cref="null"/> on further calls</returns>
        IAddress Build();
    }
}
