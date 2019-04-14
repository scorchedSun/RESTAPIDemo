namespace Contracts
{
    /// <summary>
    /// Defines functionality to create new instances of <see cref="IAddressBuilder"/> 
    /// implementations.
    /// </summary>
    public interface IAddressBuilderFactory
    {
        /// <summary>
        /// Create a new instance of an <see cref="IAddressBuilder"/> implementation.
        /// </summary>
        /// <returns>An instance of an <see cref="IAddressBuilder"/> implementation</returns>
        IAddressBuilder Create();
    }
}
