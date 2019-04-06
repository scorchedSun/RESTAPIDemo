using System.Collections.Generic;

namespace Contracts
{
    /// <summary>
    /// Defines functionality to convert between two types.
    /// </summary>
    /// <typeparam name="T1">First type</typeparam>
    /// <typeparam name="T2">Second type</typeparam>
    public interface IConverter<T1, T2>
    {
        /// <summary>
        /// Converts an instance of <typeparamref name="T1"/> to an instance of <typeparamref name="T2"/>.
        /// </summary>
        /// <param name="toConvert">The instance to convert</param>
        /// <returns>The converted instance</returns>
        T2 Convert(T1 toConvert);

        /// <summary>
        /// Converts a <see cref="IList{T1}"/> to a <see cref="IList{T2}"/>.
        /// </summary>
        /// <param name="toConvert">The instances to convert</param>
        /// <returns>The converted instances</returns>
        IList<T2> Convert(IList<T1> toConvert);

        /// <summary>
        /// Converts an instance of <typeparamref name="T2"/> to an instance of <typeparamref name="T1"/>.
        /// </summary>
        /// <param name="toConvert">The instance to convert</param>
        /// <returns>The converted instance</returns>
        T1 Convert(T2 toConvert);

        /// <summary>
        /// Converts a <see cref="IList{T2}"/> to a <see cref="IList{T1}"/>.
        /// </summary>
        /// <param name="toConvert">The instances to convert</param>
        /// <returns>The converted instances</returns>
        IList<T1> Convert(IList<T2> toConvert);
    }
}
