using System;

namespace Contracts
{
    public interface IDataSourceConfiguration
    {
        string Path { get; }
        Type Type { get; }
        string Name { get; }
    }
}
