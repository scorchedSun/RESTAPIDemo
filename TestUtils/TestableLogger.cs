using Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TestUtils
{
    public class TestableLogger : ILogger
    {
        public IList<string> RegularEntries { get; } = new List<string>();
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
