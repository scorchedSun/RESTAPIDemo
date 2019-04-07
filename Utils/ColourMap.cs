using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Utils.Exceptions;

namespace Utils
{
    public static class ColourMap
    {
        private static IDictionary<string, Color> Map { get; } = new Dictionary<string, Color>()
        {
            ["1"] = Color.Blue,
            ["2"] = Color.Green,
            ["3"] = Color.Violet,
            ["4"] = Color.Red,
            ["5"] = Color.Yellow,
            ["6"] = Color.Turquoise,
            ["7"] = Color.White
        };

        public static Color GetColourFor(string code)
        {
            if (!IsValidColourCode(code)) throw new InvalidColourCodeException(code);
            return Map[code];
        }

        public static string GetCodeFor(Color colour)
        {
            if (!IsSupportedColour(colour)) throw new UnsupportedColourException(colour);
            return Map.Single(kvp => kvp.Value == colour).Key;
        }

        public static IList<string> GetAllCodes() => Map.Keys.ToList();

        private static bool IsValidColourCode(string code) => Map.ContainsKey(code);
        private static bool IsSupportedColour(Color colour) => Map.Any(kvp => kvp.Value == colour);
    }
}
