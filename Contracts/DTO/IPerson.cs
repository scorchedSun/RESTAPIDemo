using System.Drawing;

namespace Contracts
{
    public interface IPerson : IIdentifyable
    {
        string Name { get; }
        string LastName { get; }
        IAddress Address { get; }
        Color FavouriteColour { get; }
    }
}
