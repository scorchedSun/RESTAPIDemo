using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Contracts
{
    public interface IPersonRepository : IRepository<IPerson>
    {
        IList<IPerson> GetByFavouriteColour(Color colour);
    }
}
