using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Contracts
{
    public interface IWithIDPersonBuilder
    {
        IWithNamePersonBuilder WithID(int id);
    }

    public interface IWithNamePersonBuilder
    {
        IWithLastNamePersonBuilder WithName(string name);
    }

    public interface IWithLastNamePersonBuilder
    {
        IWithAddressPersonBuilder WithLastName(string lastName);
    }

    public interface IWithAddressPersonBuilder
    {
        IWithFavouriteColourPersonBuilder WithAddress(IAddress address);
    }

    public interface IWithFavouriteColourPersonBuilder
    {
        IPersonBuilder WithFavouriteColour(Color colour);
    }

    public interface IPersonBuilder 
        : IWithIDPersonBuilder, IWithNamePersonBuilder, IWithLastNamePersonBuilder, IWithAddressPersonBuilder, IWithFavouriteColourPersonBuilder
    {
        IPerson Build();
    }
}
