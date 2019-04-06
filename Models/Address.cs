using Contracts;

namespace Models
{
    public class Address : IAddress
    {
        public string ZipCode{ get; }

        public string City{ get; }

        public Address(string zipCode, string city)
        {
            ZipCode = zipCode;
            City = city;
        }
    }
}
