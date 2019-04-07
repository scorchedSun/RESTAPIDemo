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

        public static IPersonWithID Create() => new PersonBuilder();

        public IPerson Build()
        {
            return new Person(id, name, lastName, address, colour);
        }

        public IPersonWithFavouriteColour WithAddress(IAddress address)
        {
            this.address = address;
            return this;
        }

        public IPersonBuilder WithFavouriteColour(Color colour)
        {
            this.colour = colour;
            return this;
        }

        public IPersonWithName WithID(int id)
        {
            this.id = id;
            return this;
        }

        public IPersonWithAddress WithLastName(string lastName)
        {
            this.lastName = lastName;
            return this;
        }

        public IPersonWithLastName WithName(string name)
        {
            this.name = name;
            return this;
        }
    }
}
