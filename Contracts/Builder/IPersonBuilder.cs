using System.Drawing;

namespace Contracts
{
    public interface IPersonWithIDBuilder
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
        IPersonBuilder WithFavouriteColour(Color colour);
    }

    public interface IPersonBuilder
        : IPersonWithIDBuilder, IPersonWithNameBuilder, IPersonWithLastNameBuilder, IPersonWithAddressBuilder, IPersonWithFavouriteColourBuilder
    {
        IPerson Build();
    }
}
