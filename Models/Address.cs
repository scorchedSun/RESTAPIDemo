using Contracts;

namespace Models
{
    public class Address : IAddress
    {
        public string ZipCode{ get; }

        public string City{ get; }

        internal Address(string zipCode, string city)
        {
            ZipCode = zipCode;
            City = city;
        }
    }
}
