using System.Drawing;

namespace Contracts
{
    /*
     * This collection of interfaces defines the functionality of a
     * builder for IPerson. By using separate interfaces and returning
     * another interface per stage the user is forced to call all
     * necessary setters prior to calling the Build() method.
     */

    /// <summary>
    /// Entry point for building a <see cref="IPerson"/>
    /// </summary>
    public interface IPersonBuilder
    {
        /// <summary>
        /// Set the <see cref="IIdentifyable.ID"/> property of the <see cref="IPerson"/>
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>Next stage of the builder</returns>
        /// <exception cref="ArgumentOutOfRangeException">If 0 is passed</exception>
        IPersonWithNameBuilder WithID(uint id);
    }

    /// <summary>
    /// Second stage for building an <see cref="IPerson"/>
    /// </summary>
    public interface IPersonWithNameBuilder
    {
        /// <summary>
        /// Set the <see cref="IPerson.Name"/> property of the <see cref="IPerson"/>
        /// </summary>
        /// <param name="name">The person's name</param>
        /// <returns>Next stage of the builder</returns>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        /// <exception cref="ArgumentException">If <see cref="string.Empty"/> is passed</exception>
        IPersonWithLastNameBuilder WithName(string name);
    }

    /// <summary>
    /// Third stage for building a <see cref="IPerson"/>
    /// </summary>
    public interface IPersonWithLastNameBuilder
    {
        /// <summary>
        /// Set the <see cref="IPerson.LastName"/> property of the <see cref="IPerson"/>
        /// </summary>
        /// <param name="lastName">The person's last name</param>
        /// <returns>Next stage of the builder</returns>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        /// <exception cref="ArgumentException">If <see cref="string.Empty"/> is passed</exception>
        IPersonWithAddressBuilder WithLastName(string lastName);
    }

    /// <summary>
    /// Fourth stage for building a <see cref="IPerson"/>
    /// </summary>
    public interface IPersonWithAddressBuilder
    {
        /// <summary>
        /// Set the <see cref="IPerson.Address"/> property of the <see cref="IPerson"/>
        /// </summary>
        /// <param name="address">The person's address</param>
        /// <returns>Next stage of the builder</returns>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        IPersonWithFavouriteColourBuilder WithAddress(IAddress address);
    }

    /// <summary>
    /// Fifth stage for building a <see cref="IPerson"/>
    /// </summary>
    public interface IPersonWithFavouriteColourBuilder
    {
        /// <summary>
        /// Set the <see cref="IPerson.FavouriteColour"/> property of the <see cref="IPerson"/>
        /// </summary>
        /// <param name="colour">The person's favourite colour</param>
        /// <returns>Next stage of the builder</returns>
        IFinalPersonBuilder WithFavouriteColour(Color colour);
    }

    /// <summary>
    /// Last stage for building a <see cref="IPerson"/>
    /// </summary>
    public interface IFinalPersonBuilder
    {
        /// <summary>
        /// Creates a new <see cref="IPerson"/> using the values set in prior stages.
        /// </summary>
        /// <returns>An instance of a <see cref="IPerson"/> implementation on first call, <see cref="null"/> on further calls</returns>
        IPerson Build();
    }
}
