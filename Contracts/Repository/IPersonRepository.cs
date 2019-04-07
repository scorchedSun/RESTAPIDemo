using System.Collections.Generic;
using System.Drawing;

namespace Contracts
{
    /// <summary>
    /// Defines functionality for managing instances of <see cref="IPerson"/> at runtime.
    /// </summary>
    public interface IPersonRepository : IRepository<IPerson>
    {
        /// <summary>
        /// Access all instances of <see cref="IPerson"/> that share the same <see cref="IPerson.FavouriteColour"/>.
        /// </summary>
        /// <param name="colour">The colour to filter by</param>
        /// <returns><see cref="IList{IPerson}"/> containing each <see cref="IPerson"/> having <paramref name="colour"/> as their <see cref="IPerson.FavouriteColour"/></returns>
        IList<IPerson> GetByFavouriteColour(Color colour);
    }
}
