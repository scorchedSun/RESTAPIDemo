using Contracts;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Repositories
{
    /// <summary>
    /// Repository of persons
    /// </summary>
    public class PersonRepository : IPersonRepository
    {
        // Caching the persons in memory allows much faster access
        private readonly IList<IPerson> cache;

        /// <summary>
        /// Create a new <see cref="PersonRepository"/> using the <paramref name="dataSource"/>.
        /// </summary>
        /// <param name="dataSource">Data source to use</param>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        public PersonRepository(IDataSource<IPerson> dataSource)
        {
            if (dataSource is null) throw new ArgumentNullException(nameof(dataSource));
            cache = dataSource.LoadAll();
        }

        /// <summary>
        /// Gets a person by <see cref="IPerson.ID"/>.
        /// </summary>
        /// <param name="id">The person's <see cref="IPerson.ID"/></param>
        /// <returns>The person if it exists</returns>
        /// <exception cref="PersonDoesNotExistException">If no person with that <see cref="IPerson.ID"/> exists</exception>
        /// <exception cref="AmbiguousIDException">If more than one person with that <see cref="IPerson.ID"/> exists.</exception>
        public IPerson Get(uint id)
        {
            EnsureIsValid(id);
            return cache.Single(entry => entry.ID == id);
        }

        /// <summary>
        /// Get all persons.
        /// </summary>
        /// <returns>Returns all cached persons</returns>
        public IList<IPerson> GetAll() => cache;

        /// <summary>
        /// Get all persons having the same <see cref="IWithFavouriteColour.FavouriteColour"/>
        /// </summary>
        /// <param name="colour">The colour to look for</param>
        /// <returns>All cached persons having <paramref name="colour"/> as their <see cref="IWithFavouriteColour.FavouriteColour"/></returns>
        public IList<IPerson> GetByFavouriteColour(Color colour)
            => cache.Where(entry => entry.FavouriteColour == colour).ToList();

        private void EnsureIsValid(uint id)
        {
            int matches = cache.Count(entry => entry.ID == id);

            if (matches == 0)
                throw new PersonDoesNotExistException(id);
            else if (matches > 1)
                throw new AmbiguousIDException(id);
        }
    }
}
