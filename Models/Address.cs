using Contracts;

namespace Models
{
    public class Address : IAddress
    {
        public string ZipCode{ get; internal set; }

        public string City{ get; internal set; }

        internal Address() {}
    }
}
