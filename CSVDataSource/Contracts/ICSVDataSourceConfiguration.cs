using Contracts;
using System.Collections.Generic;

namespace CSVDataSource.Contracts
{
    /// <summary>
    /// Extends <see cref="IDataSourceConfiguration"/> to define additional properties needed
    /// to represent settings for data sources that are based on CSV files.
    /// </summary>
    public interface ICSVDataSourceConfiguration : IDataSourceConfiguration
    {
        string Separator { get; }
        IList<string> FieldSequence { get; }
    }
}
