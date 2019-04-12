using Contracts;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace RESTAPIDemo
{
    /// <summary>
    /// Provides basic logging functionality
    /// </summary>
    public class DiagnosticsLogger : ILogger
    {
        private const string baseFormat = "[{0}] [{1}] [{2}.{3}]: {4}";
        private const string info = "Info";
        private const string error = "Error";
        private const string stackTrace = "Stack trace:";
        private readonly object mutex = new object();

        public void Log(string message, [CallerFilePath] string path = "", [CallerMemberName] string member = "")
            => Log(info, message, path, member);

        public void LogError(string message, [CallerFilePath] string path = "", [CallerMemberName] string member = "")
            => Log(error, message, path, member);

        public void Log(Exception exception, [CallerFilePath] string path = "", [CallerMemberName] string member = "")
            => Log("", exception, path, member);

        public void Log(string message, Exception exception, [CallerFilePath] string path = "", [CallerMemberName] string member = "")
            => Log(error, message + (message.EndsWith(" ") ? "" : " ") + FormatException(exception), path, member);

        private void Log(string level, string message, string path, string member)
        {
            lock (mutex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format(
                    baseFormat,
                    level,
                    NowAsUniversal,
                    GetFileName(path),
                    member,
                    message));
            }
        }

        private string GetFileName(string path) => Path.GetFileNameWithoutExtension(path);

        private DateTime NowAsUniversal => DateTime.Now.ToUniversalTime();

        private string FormatException(Exception exception)
            => exception.Message + Environment.NewLine + stackTrace + Environment.NewLine + exception.StackTrace;
    }
}
