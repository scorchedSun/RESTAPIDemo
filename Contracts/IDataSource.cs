using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IDataSource<T>
    {
        IList<T> LoadAll();

        void WriteAll(IList<T> entries);
    }
}
