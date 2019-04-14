namespace Contracts
{
    /// <summary>
    /// Defines configuration values for instances of <see cref="IDataSource{T}"/>
    /// </summary>
    public interface IDataSourceConfiguration
    {
        string Path { get; }
        string Name { get; }
    }
}
