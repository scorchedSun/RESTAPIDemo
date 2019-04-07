using Contracts;
using System.Collections.Generic;

namespace TestUtils
{
    public class TestablePersonDataSource : IDataSource<IPerson>
    {
        public IList<IPerson> Persons { get; set; } = new List<IPerson>();

        public IList<IPerson> LoadAll() => Persons;

        public void WriteAll(IList<IPerson> entries) => Persons = entries;
    }
}
