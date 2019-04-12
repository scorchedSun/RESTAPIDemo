using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Contracts
{
    public interface ILogger
    {
        void Log(string message, [CallerFilePath] string path = "", [CallerMemberName] string member = "");
        void LogError(string message, [CallerFilePath] string path = "", [CallerMemberName] string member = "");
        void Log(Exception exception, [CallerFilePath] string path = "", [CallerMemberName] string member = "");
        void Log(string message, Exception exception, [CallerFilePath] string path = "", [CallerMemberName] string member = "");
    }
}
