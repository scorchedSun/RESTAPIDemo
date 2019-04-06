using Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Converters
{
    public class ColourConverter : Converter<string, Color>
    {
        public IDictionary<string, Color> ColourMap { get; } = new Dictionary<string, Color>()
        {
            ["1"] = Color.Blue,
            ["2"] = Color.Green,
            ["3"] = Color.Violet,
            ["4"] = Color.Red,
            ["5"] = Color.Yellow,
            ["6"] = Color.Turquoise,
            ["7"] = Color.White
        };

        public override Color Convert(string toConvert)
        {
            if (toConvert is null) throw new ArgumentNullException(nameof(toConvert));

            if (!IsValidColourCode(toConvert)) throw new InvalidColourCodeException(toConvert);
            return ColourMap[toConvert];
        }

        public override string Convert(Color toConvert)
        {
            if (!IsSupportedColour(toConvert)) throw new UnsupportedColourException(toConvert);
            return ColourMap.Single(kvp => kvp.Value == toConvert).Key;
        }

        private bool IsValidColourCode(string code) => ColourMap.ContainsKey(code);
        private bool IsSupportedColour(Color colour) => ColourMap.Any(kvp => kvp.Value == colour);
    }
}
