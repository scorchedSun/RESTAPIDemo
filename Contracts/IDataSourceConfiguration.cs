using Microsoft.Extensions.Configuration;
using System;

namespace Contracts
{
    public interface IDataSourceConfiguration
    {
        string Path { get; }
        string Name { get; }
    }
}
