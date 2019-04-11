using Contracts;
using Models.Builders;

namespace Models.Factories
{
    public class PersonBuilderFactory : IPersonBuilderFactory
    {
        public IPersonBuilder Create() => PersonBuilder.Create();
    }
}
