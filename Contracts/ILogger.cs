using System;
using System.Runtime.CompilerServices;

namespace Contracts
{
    /// <summary>
    /// Defines functionality that is necessary to write useful
    /// log entries.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Write a information message to the log.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="path">Path of the file the call originated from (filled at runtime through attribute)</param>
        /// <param name="member">Caller member's name (filled at runtime through attribute)</param>
        void Log(string message, [CallerFilePath] string path = "", [CallerMemberName] string member = "");

        /// <summary>
        /// Write a error message to the log.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="path">Path of the file the call originated from (filled at runtime through attribute)</param>
        /// <param name="member">Caller member's name (filled at runtime through attribute)</param>
        void LogError(string message, [CallerFilePath] string path = "", [CallerMemberName] string member = "");

        /// <summary>
        /// Write an exception to the log.
        /// </summary>
        /// <param name="exception">The exception</param>
        /// <param name="path">Path of the file the call originated from (filled at runtime through attribute)</param>
        /// <param name="member">Caller member's name (filled at runtime through attribute)</param>
        void Log(Exception exception, [CallerFilePath] string path = "", [CallerMemberName] string member = "");

        /// <summary>
        /// Write an exception alongside with an additional message
        /// to the log.
        /// </summary>
        /// <param name="message">The additional message</param>
        /// <param name="exception">The exception</param>
        /// <param name="path">Path of the file the call originated from (filled at runtime through attribute)</param>
        /// <param name="member">Caller member's name (filled at runtime through attribute)</param>
        void Log(string message, Exception exception, [CallerFilePath] string path = "", [CallerMemberName] string member = "");
    }
}
