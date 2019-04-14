namespace Contracts
{
    /// <summary>
    /// Defines properties needed to be considered having an address
    /// </summary>
    public interface IWithAddress
    {
        IAddress Address { get; }
    }
}
