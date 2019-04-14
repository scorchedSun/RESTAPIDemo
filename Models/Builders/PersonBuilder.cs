using System;
using System.Drawing;
using Contracts;

namespace Models.Builders
{
    /// <summary>
    /// Provides functionality to build new instances of <see cref="Person"/>.
    /// Each instance of <see cref="PersonBuilder"/> is one time use only, each
    /// subsequent call on <see cref="Build"/> returns null after the first.
    /// </summary>
    public sealed class PersonBuilder
        : IPersonBuilder
        , IPersonWithNameBuilder
        , IPersonWithLastNameBuilder
        , IPersonWithAddressBuilder
        , IPersonWithFavouriteColourBuilder
        , IFinalPersonBuilder
    {
        private const string idMayNotBeZeroMessage = "Needs to be larger than 0.";

        private Person person;

        // Enforce usage of factory method to allow for returning the desired first interface
        private PersonBuilder() { person = new Person(); }

        /// <summary>
        /// Creates a new <see cref="PersonBuilder"/>
        /// </summary>
        /// <returns>A new <see cref="PersonBuilder"/> representing the first stage of the builder</returns>
        internal static IPersonBuilder Create() => new PersonBuilder();

        /// <summary>
        /// Creates a new <see cref="Person"/> using the values set in prior stages.
        /// </summary>
        /// <returns>An instance of a <see cref="Person"/> implementation on first call, <see cref="null"/> on further calls</returns>
        public IPerson Build()
        {
            var result = person;
            person = null;
            return result;
        }

        /// <summary>
        /// Set the <see cref="Person.Address"/> property of the <see cref="Person"/>
        /// </summary>
        /// <param name="address">The person's address</param>
        /// <returns>Next stage of the builder</returns>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        public IPersonWithFavouriteColourBuilder WithAddress(IAddress address)
        {
            if (address is null) throw new ArgumentNullException(nameof(address));
            person.Address = address;
            return this;
        }

        /// <summary>
        /// Set the <see cref="Person.FavouriteColour"/> property of the <see cref="Person"/>
        /// </summary>
        /// <param name="colour">The person's favourite colour</param>
        /// <returns>Next stage of the builder</returns>
        public IFinalPersonBuilder WithFavouriteColour(Color colour)
        {
            person.FavouriteColour = colour;
            return this;
        }

        /// <summary>
        /// Set the <see cref="Person.ID"/> property of the <see cref="Person"/>
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>Next stage of the builder</returns>
        /// <exception cref="ArgumentOutOfRangeException">If 0 is passed</exception>
        public IPersonWithNameBuilder WithID(uint id)
        {
            if (id == 0) throw new ArgumentOutOfRangeException(nameof(id), idMayNotBeZeroMessage);
            person.ID = id;
            return this;
        }

        /// <summary>
        /// Set the <see cref="Person.LastName"/> property of the <see cref="Person"/>
        /// </summary>
        /// <param name="lastName">The person's last name</param>
        /// <returns>Next stage of the builder</returns>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        /// <exception cref="ArgumentException">If <see cref="string.Empty"/> is passed</exception>
        public IPersonWithAddressBuilder WithLastName(string lastName)
        {
            if (lastName is null) throw new ArgumentNullException(nameof(lastName));
            if (lastName.Equals(string.Empty)) throw new ArgumentException(nameof(lastName));
            person.LastName = lastName;
            return this;
        }

        /// <summary>
        /// Set the <see cref="Person.Name"/> property of the <see cref="Person"/>
        /// </summary>
        /// <param name="name">The person's name</param>
        /// <returns>Next stage of the builder</returns>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        /// <exception cref="ArgumentException">If <see cref="string.Empty"/> is passed</exception>
        public IPersonWithLastNameBuilder WithName(string name)
        {
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (name.Equals(string.Empty)) throw new ArgumentException(nameof(name));
            person.Name = name;
            return this;
        }
    }
}
