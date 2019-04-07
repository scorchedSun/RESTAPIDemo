using Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Converters
{
    public abstract class Converter<T1, T2> : IConverter<T1, T2>
    {
        public abstract T2 Convert(T1 toConvert);

        public IList<T2> Convert(IList<T1> toConvert) => toConvert.Select(Convert).ToList();

        public abstract T1 Convert(T2 toConvert);

        public IList<T1> Convert(IList<T2> toConvert) => toConvert.Select(Convert).ToList();
    }
}
