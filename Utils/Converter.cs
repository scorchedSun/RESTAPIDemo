using Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    /// <summary>
    /// Base class for converting between two types.
    /// </summary>
    /// <typeparam name="T1">First type</typeparam>
    /// <typeparam name="T2">Second type</typeparam>
    public abstract class Converter<T1, T2> : IConverter<T1, T2>
    {
        /// <summary>
        /// Converts an instance of <typeparamref name="T1"/> to an instance of <typeparamref name="T2"/>.
        /// </summary>
        /// <param name="toConvert">The instance to convert</param>
        /// <returns>The converted instance</returns>
        public abstract T2 Convert(T1 toConvert);

        /// <summary>
        /// Converts a <see cref="IList{T1}"/> to a <see cref="IList{T2}"/>.
        /// </summary>
        /// <param name="toConvert">The instances to convert</param>
        /// <returns>The converted instances</returns>
        public IList<T2> Convert(IList<T1> toConvert) => toConvert.Select(Convert).ToList();

        /// <summary>
        /// Converts an instance of <typeparamref name="T2"/> to an instance of <typeparamref name="T1"/>.
        /// </summary>
        /// <param name="toConvert">The instance to convert</param>
        /// <returns>The converted instance</returns>
        public abstract T1 Convert(T2 toConvert);

        /// <summary>
        /// Converts a <see cref="IList{T2}"/> to a <see cref="IList{T1}"/>.
        /// </summary>
        /// <param name="toConvert">The instances to convert</param>
        /// <returns>The converted instances</returns>
        public IList<T1> Convert(IList<T2> toConvert) => toConvert.Select(Convert).ToList();
    }
}
