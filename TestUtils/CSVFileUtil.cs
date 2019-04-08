using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestUtils
{
    public static class CSVFileUtil
    {
        private const string SeparatorSequence = ", ";

        private static readonly IList<string> testData = new List<string>()
        {
            "Müller, Hans, 67742 Lauterecken, 1",
            "Petersen, Peter, 18439 Stralsund, 2",
            "Johnson, Johnny, 88888 made up, 3",
            "Millenium, Milly, 77777 made up too, 4",
            "Müller, Jonas, 32323 Hansstadt, 5",
            "Fujitsu, Tastatur, 42342 Japan, 6",
            "Andersson, Anders, 32132 Schweden - Bonus, 2",
            "Bart, Bertram, 12313 Wasweißich, 1",
            "Gerber, Gerda, 76535 Woanders, 3",
            "Klaussen, Klaus, 43246 Hierach, 2"
        };

        private static readonly IList<string> fieldSequence = new List<string> { "LastName", "Name", "Address", "FavouriteColour" };

        public static int NumberOfEntries => testData.Count;

        public static string CreateTestFile()
        {
            string filePath = Path.GetTempFileName() + ".csv";
            File.WriteAllLines(filePath, testData);
            return filePath;
        }

        public static string GetNonExistantFile() => Path.GetRandomFileName() + ".csv";

        public static bool EntryExists(string entry) => testData.Any(value => value.Equals(entry));

        public static int IndexOf(string entry) => testData.IndexOf(entry);

        public static MockCSVDataSourceConfiguration CreateMockConfiguration()
        {
            return new MockCSVDataSourceConfiguration()
            {
                Separator = SeparatorSequence,
                FieldSequence = fieldSequence
            };
        }
    }
}
