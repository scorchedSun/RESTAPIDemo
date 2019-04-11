using System;
using System.Drawing;
using Contracts;

namespace Models.Builders
{
    public sealed class PersonBuilder
        : IPersonBuilder
        , IPersonWithNameBuilder
        , IPersonWithLastNameBuilder
        , IPersonWithAddressBuilder
        , IPersonWithFavouriteColourBuilder
        , IFinalPersonBuilder
    {
        private Person person;

        private PersonBuilder() { person = new Person(); }

        public static IPersonBuilder Create() => new PersonBuilder();

        public IPerson Build()
        {
            var result = person;
            person = null;
            return result;
        }

        public IPersonWithFavouriteColourBuilder WithAddress(IAddress address)
        {
            if (address is null) throw new ArgumentNullException(nameof(address));
            person.Address = address;
            return this;
        }

        public IFinalPersonBuilder WithFavouriteColour(Color colour)
        {
            person.FavouriteColour = colour;
            return this;
        }

        public IPersonWithNameBuilder WithID(int id)
        {
            person.ID = id;
            return this;
        }

        public IPersonWithAddressBuilder WithLastName(string lastName)
        {
            if (lastName is null) throw new ArgumentNullException(nameof(lastName));
            if (lastName.Equals(string.Empty)) throw new ArgumentException(nameof(lastName));
            person.LastName = lastName;
            return this;
        }

        public IPersonWithLastNameBuilder WithName(string name)
        {
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (name.Equals(string.Empty)) throw new ArgumentException(nameof(name));
            person.Name = name;
            return this;
        }
    }
}
