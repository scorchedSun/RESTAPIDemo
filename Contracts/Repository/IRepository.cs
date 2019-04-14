using System.Collections.Generic;

namespace Contracts
{
    /// <summary>
    /// Defines functionality for managing instances of <typeparamref name="T"/> at runtime.
    /// </summary>
    /// <typeparam name="T">Type to manage</typeparam>
    public interface IRepository<T>
        where T : IIdentifyable
    {
        /// <summary>
        /// Access all stored instances.
        /// </summary>
        /// <returns>All instances of this repository</returns>
        IList<T> GetAll();

        /// <summary>
        /// Access a specific instance of <typeparamref name="T"/> by its <see cref="IIdentifyable.ID"/>.
        /// </summary>
        /// <param name="id">The instance's ID</param>
        /// <returns>The instance</returns>
        T Get(uint id);
    }
}
