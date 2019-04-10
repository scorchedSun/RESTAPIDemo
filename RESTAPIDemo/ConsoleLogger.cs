using Contracts;
using System;

namespace RESTAPIDemo
{
    /// <summary>
    /// Provides basic logging functionality
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        private const string baseFormat = "[{0}] [{1}]: [{2}]";
        private readonly object mutex = new object();

        public void Log(string message)
        {
            lock (mutex)
            {
                Console.WriteLine(string.Format(baseFormat, "Info", DateTime.Now.ToUniversalTime(), message));
            }
        }

        public void Log(Exception exception)
        {
            lock (mutex)
            {
                Console.WriteLine(string.Format(baseFormat, "Error", DateTime.Now.ToUniversalTime(), exception.Message + Environment.NewLine + "Stack trace:" + Environment.NewLine + exception.StackTrace));
            }
        }
    }
}
