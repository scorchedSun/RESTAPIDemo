using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Contracts;

namespace Models.Builders
{
    public sealed class PersonBuilder : IPersonBuilder
    {
        private int id;
        private string name;
        private string lastName;
        private IAddress address;
        private Color colour;

        private PersonBuilder() {}

        public static IWithIDPersonBuilder Create() => new PersonBuilder();

        public IPerson Build()
        {
            return new Person(id, name, lastName, address, colour);
        }

        public IWithFavouriteColourPersonBuilder WithAddress(IAddress address)
        {
            this.address = address;
            return this;
        }

        public IPersonBuilder WithFavouriteColour(Color colour)
        {
            this.colour = colour;
            return this;
        }

        public IWithNamePersonBuilder WithID(int id)
        {
            this.id = id;
            return this;
        }

        public IWithAddressPersonBuilder WithLastName(string lastName)
        {
            this.lastName = lastName;
            return this;
        }

        public IWithLastNamePersonBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }
    }
}
