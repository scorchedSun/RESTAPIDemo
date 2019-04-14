using Contracts;
using Models.Builders;
using System.Collections.Generic;
using System.Drawing;

namespace TestUtils
{
    public class TestablePersonDataSource : IDataSource<IPerson>
    {
        public static uint InvalidID { get; } = 0u;
        public static uint ExistingID { get; } = 1u;
        public static uint AmbiguousID { get; } = 2u;
        public static Color InvalidColour { get; } = Color.Transparent;
        public static Color ValidColour { get; } = Color.Blue;

        public IList<IPerson> Persons { get; set; } = new List<IPerson>();

        public IList<IPerson> LoadAll() => Persons;

        public void WriteAll(IList<IPerson> entries) => Persons = entries;

        public static TestablePersonDataSource Create()
        {
            TestablePersonDataSource dataSource = new TestablePersonDataSource();
            dataSource.WriteAll(new List<IPerson>
            {
                CreateTestPerson(ExistingID, ValidColour),
                CreateTestPerson(AmbiguousID, Color.Black),
                CreateTestPerson(AmbiguousID, Color.Azure)
            });
            return dataSource;
        }

        private static IAddress CreateTestAddress()
        {
            IAddressBuilder addressBuilder = AddressBuilder.Create();
            return addressBuilder
                .WithZipCode("32423")
                .WithCity("Test City")
                .Build();
        }

        private static IPerson CreateTestPerson(uint id, Color colour)
        {
            IPersonBuilder personBuilder = PersonBuilder.Create();
            return personBuilder
                .WithID(id)
                .WithName("test")
                .WithLastName("tester")
                .WithAddress(CreateTestAddress())
                .WithFavouriteColour(colour)
                .Build();
        }
    }
}
