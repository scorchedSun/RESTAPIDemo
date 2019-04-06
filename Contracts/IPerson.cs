using System;
using System.Drawing;

namespace Contracts
{
    public interface IPerson : IIdentifyable
    {
        string Name { get; }
        string LastName { get; }
        string ZipCode { get; }
        string City { get; }
        Color FavouriteColour { get; }
    }
}
