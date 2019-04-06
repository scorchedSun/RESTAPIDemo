using System.Collections.Generic;

namespace Contracts
{
    /// <summary>
    /// Defines functionality for storing data.
    /// </summary>
    /// <typeparam name="T">Type to store</typeparam>
    public interface IDataSource<T>
    {
        /// <summary>
        /// Loads all entries from the data store.
        /// </summary>
        /// <returns>All entries contained in the data store</returns>
        IList<T> LoadAll();

        /// <summary>
        /// Writes all entries to the data store.
        /// </summary>
        /// <param name="entries"><see cref="IList{T}"/> of instances to be written to the data store</param>
        void WriteAll(IList<T> entries);
    }
}
