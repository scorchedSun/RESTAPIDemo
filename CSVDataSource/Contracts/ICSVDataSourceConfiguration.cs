using Contracts;
using System.Collections.Generic;

namespace CSVDataSource.Contracts
{
    public interface ICSVDataSourceConfiguration : IDataSourceConfiguration
    {
        string Separator { get; }
        IList<string> FieldSequence { get; }
    }
}
