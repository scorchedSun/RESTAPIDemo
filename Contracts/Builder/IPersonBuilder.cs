using System.Drawing;

namespace Contracts
{
    public interface IPersonWithID
    {
        IPersonWithName WithID(int id);
    }

    public interface IPersonWithName
    {
        IPersonWithLastName WithName(string name);
    }

    public interface IPersonWithLastName
    {
        IPersonWithAddress WithLastName(string lastName);
    }

    public interface IPersonWithAddress
    {
        IPersonWithFavouriteColour WithAddress(IAddress address);
    }

    public interface IPersonWithFavouriteColour
    {
        IPersonBuilder WithFavouriteColour(Color colour);
    }

    public interface IPersonBuilder
        : IPersonWithID, IPersonWithName, IPersonWithLastName, IPersonWithAddress, IPersonWithFavouriteColour
    {
        IPerson Build();
    }
}
