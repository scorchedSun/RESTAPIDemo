using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Contracts;

namespace Models.Builders
{
    public sealed class PersonBuilder : IFinalPersonBuilder
    {
        private int id;
        private string name;
        private string lastName;
        private IAddress address;
        private Color colour;

        private PersonBuilder() {}

        public static IPersonBuilder Create() => new PersonBuilder();

        public IPerson Build()
        {
            return new Person(id, name, lastName, address, colour);
        }

        public IPersonWithFavouriteColourBuilder WithAddress(IAddress address)
        {
            this.address = address;
            return this;
        }

        public IFinalPersonBuilder WithFavouriteColour(Color colour)
        {
            this.colour = colour;
            return this;
        }

        public IPersonWithNameBuilder WithID(int id)
        {
            this.id = id;
            return this;
        }

        public IPersonWithAddressBuilder WithLastName(string lastName)
        {
            this.lastName = lastName;
            return this;
        }

        public IPersonWithLastNameBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }
    }
}
