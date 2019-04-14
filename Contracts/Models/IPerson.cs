namespace Contracts
{
    /// <summary>
    /// Defines all properties a person needs
    /// </summary>
    public interface IPerson : IIdentifyable, IWithAddress, IWithFavouriteColour
    {
        string Name { get; }
        string LastName { get; }
    }
}
