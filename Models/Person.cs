using Contracts;
using System.Drawing;

namespace Models
{
    /// <summary>
    /// Represents a person.
    /// </summary>
    public class Person : IPerson
    {
        public string Name { get; internal set; }

        public string LastName { get; internal set; }

        public IAddress Address { get; internal set; }

        public Color FavouriteColour { get; internal set; }

        public uint ID { get; internal set; }

        /// <summary>
        /// Should never be instantiated directly. Use <see cref="PersonBuilder"/>
        /// </summary>
        internal Person() {}
    }
}
