using Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TestUtils
{
    /// <summary>
    /// Mock class for checking log entries in unit tests.
    /// Writes messages into public lists so that they can be read externally.
    /// </summary>
    public class TestableLogger : ILogger
    {
        /// <summary>
        /// Entries other than errors
        /// </summary>
        public IList<string> RegularEntries { get; } = new List<string>();
        /// <summary>
        /// Entries that are error messages
        /// </summary>
        public IList<string> Errors { get; } = new List<string>();

        public void Log(string message, [CallerFilePath] string path = "", [CallerMemberName] string member = "")
            => RegularEntries.Add(message);

        public void Log(Exception exception, [CallerFilePath] string path = "", [CallerMemberName] string member = "")
            => Errors.Add(exception.Message);

        public void Log(string message, Exception exception, [CallerFilePath] string path = "", [CallerMemberName] string member = "")
            => Errors.Add(message + exception.Message);

        public void LogError(string message, [CallerFilePath] string path = "", [CallerMemberName] string member = "")
            => Errors.Add(message);
    }
}
