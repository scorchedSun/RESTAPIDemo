using Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Utils
{
    /// <summary>
    /// Defines functionality around converting between colours and their codes.
    /// </summary>
    public static class ColourMap
    {
        /// <summary>
        /// Defines the colour supported by the system and their corresponding code.
        /// </summary>
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

        /// <summary>
        /// Gets the colour for the <paramref name="code"/>.
        /// </summary>
        /// <param name="code">The code</param>
        /// <returns>The matching colour</returns>
        /// <exception cref="InvalidColourCodeException"><paramref name="code"/> was not in the list of supported colour codes</exception>
        public static Color GetColourFor(string code)
        {
            if (!IsValidColourCode(code)) throw new InvalidColourCodeException(code);
            return Map[code];
        }

        /// <summary>
        /// Gets the colour by its <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>The matching colour</returns>
        /// <exception cref="InvalidColourNameException">The supported colours don't contain a colour with a matching <paramref name="name"/></exception>
        public static Color GetColourByName(string name)
        {
            if (!ColourWithNameExists(name)) throw new InvalidColourNameException(name);
            return Map.Values.Single(v => v.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Gets the code for a <paramref name="colour"/>.
        /// </summary>
        /// <param name="colour">The colour</param>
        /// <returns>The colour's code</returns>
        /// <exception cref="UnsupportedColourException"><paramref name="colour"/> was not in the list of supported colours</exception>
        public static string GetCodeFor(Color colour)
        {
            if (!IsSupportedColour(colour)) throw new UnsupportedColourException(colour);
            return Map.Single(kvp => kvp.Value == colour).Key;
        }

        /// <summary>
        /// Get all supported colour codes.
        /// </summary>
        /// <returns>The supported colour codes</returns>
        public static IList<string> GetAllCodes() => Map.Keys.ToList();

        private static bool IsValidColourCode(string code) => Map.ContainsKey(code);
        private static bool IsSupportedColour(Color colour) => Map.Any(kvp => kvp.Value == colour);
        private static bool ColourWithNameExists(string name) => Map.Values.Any(v => v.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
    }
}
