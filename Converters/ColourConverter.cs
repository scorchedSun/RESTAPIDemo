using System;
using System.Drawing;
using Utils;

namespace Converters
{
    public class ColourConverter : Converter<string, Color>
    {
        public override Color Convert(string toConvert)
        {
            if (toConvert is null) throw new ArgumentNullException(nameof(toConvert));
            return ColourMap.GetColourFor(toConvert);
        }

        public override string Convert(Color toConvert) => ColourMap.GetCodeFor(toConvert);
    }
}
