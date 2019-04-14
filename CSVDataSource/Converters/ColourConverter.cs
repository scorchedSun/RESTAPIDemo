using System.Drawing;
using Utils;
using System;

namespace CSVDataSource.Converters
{
    /// <summary>
    /// Converts a <see cref="string"/> to an <see cref="Color"/> and vice versa.
    /// </summary>
    public class ColourConverter : Utils.Converter<string, Color>
    {
        /// <summary>
        /// Convert a <see cref="string"/> to an <see cref="Color"/>.
        /// </summary>
        /// <param name="toConvert">The <see cref="string"/> to convert</param>
        /// <returns>The converted <see cref="string"/></returns>
        /// <exception cref="ArgumentNullException">If <see cref="null"/> is passed</exception>
        public override Color Convert(string toConvert)
        {
            if (toConvert is null) throw new ArgumentNullException(nameof(toConvert));
            return ColourMap.GetColourFor(toConvert);
        }

        /// <summary>
        /// Convert an <see cref="Color"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="toConvert">The <see cref="Color"/> to convert</param>
        /// <returns>The converted <see cref="Color"/></returns>
        public override string Convert(Color toConvert) => ColourMap.GetCodeFor(toConvert);
    }
}
