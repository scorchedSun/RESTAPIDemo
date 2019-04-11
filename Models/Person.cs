using Contracts;
using System.Drawing;

namespace Models
{
    public class Person : IPerson
    {
        public string Name { get; internal set; }

        public string LastName { get; internal set; }

        public IAddress Address { get; internal set; }

        public Color FavouriteColour { get; internal set; }

        public int ID { get; internal set; }

        internal Person() {}
    }
}
