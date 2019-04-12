using Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Utils.Exceptions;

namespace Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IList<IPerson> cache;

        public PersonRepository(IDataSource<IPerson> dataSource)
        {
            if (dataSource is null) throw new ArgumentNullException(nameof(dataSource));
            cache = dataSource.LoadAll();
        }

        public IPerson Get(int id)
        {
            EnsureIsValid(id);
            return cache.Single(entry => entry.ID == id);
        }

        public IList<IPerson> GetAll() => cache;

        public IList<IPerson> GetByFavouriteColour(Color colour)
            => cache.Where(entry => entry.FavouriteColour == colour).ToList();

        private void EnsureIsValid(int id)
        {
            int matches = cache.Count(entry => entry.ID == id);

            if (matches == 0)
                throw new PersonDoesNotExistException(id);
            else if (matches > 1)
                throw new AmbiguousIDException(id);
        }
    }
}
