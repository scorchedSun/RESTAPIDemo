using System.Drawing;

namespace Contracts
{
    /// <summary>
    /// Defines properties needed to be considered having a favourite colour
    /// </summary>
    public interface IWithFavouriteColour
    {
        Color FavouriteColour { get; }
    }
}
