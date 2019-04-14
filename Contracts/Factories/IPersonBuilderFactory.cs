namespace Contracts
{
    /// <summary>
    /// Defines functionality to create new instances of <see cref="IPersonBuilder"/> 
    /// implementations.
    /// </summary>
    public interface IPersonBuilderFactory
    {
        /// <summary>
        /// Create a new instance of an <see cref="IPersonBuilder"/> implementation.
        /// </summary>
        /// <returns>An instance of an <see cref="IPersonBuilder"/> implementation</returns>
        IPersonBuilder Create();
    }
}
