using System.Drawing;
using Utils;
using System;

namespace CSVDataSource.Converters
{
    public class ColourConverter : Utils.Converter<string, Color>
    {
        public override Color Convert(string toConvert)
        {
            if (toConvert is null) throw new ArgumentNullException(nameof(toConvert));
            return ColourMap.GetColourFor(toConvert);
        }

        public override string Convert(Color toConvert) => ColourMap.GetCodeFor(toConvert);
    }
}
