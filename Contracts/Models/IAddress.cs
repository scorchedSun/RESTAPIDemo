namespace Contracts
{
    /// <summary>
    /// Defines all properties an address needs
    /// </summary>
    public interface IAddress
    {
        string ZipCode { get; }
        string City { get; }
    }
}
