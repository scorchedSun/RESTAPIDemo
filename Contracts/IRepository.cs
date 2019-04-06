using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRepository<T>
        where T : IIdentifyable
    {
        IList<T> GetAll();

        T Get(int id);
    }
}
