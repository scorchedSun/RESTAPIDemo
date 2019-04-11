using System.Drawing;

namespace Contracts
{
    public interface IPersonBuilder
    {
        IPersonWithNameBuilder WithID(int id);
    }

    public interface IPersonWithNameBuilder
    {
        IPersonWithLastNameBuilder WithName(string name);
    }

    public interface IPersonWithLastNameBuilder
    {
        IPersonWithAddressBuilder WithLastName(string lastName);
    }

    public interface IPersonWithAddressBuilder
    {
        IPersonWithFavouriteColourBuilder WithAddress(IAddress address);
    }

    public interface IPersonWithFavouriteColourBuilder
    {
        IFinalPersonBuilder WithFavouriteColour(Color colour);
    }

    public interface IFinalPersonBuilder
    {
        IPerson Build();
    }
}
