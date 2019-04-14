using Contracts;
using Models.Builders;

namespace Models.Factories
{
    /// <summary>
    /// Responsible for creating new instances of <see cref="PersonBuilder"/>
    /// </summary>
    public class PersonBuilderFactory : IPersonBuilderFactory
    {
        /// <summary>
        /// Create a new <see cref="PersonBuilder"/>
        /// </summary>
        /// <returns>A <see cref="PersonBuilder"/></returns>
        public IPersonBuilder Create() => PersonBuilder.Create();
    }
}
