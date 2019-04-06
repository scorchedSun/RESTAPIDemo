using Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Converters
{
    public abstract class Converter<TSource, TTarget> : IConverter<TSource, TTarget>
    {
        public abstract TTarget Convert(TSource toConvert);

        public IList<TTarget> Convert(IList<TSource> toConvert) => toConvert.Select(Convert).ToList();

        public abstract TSource Convert(TTarget toConvert);

        public IList<TSource> Convert(IList<TTarget> toConvert) => toConvert.Select(Convert).ToList();
    }
}
