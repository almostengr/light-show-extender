using System;
using static System.Console;

namespace Almostengr.FalconPiMonitor
{
    public static class Logger
    {
        public static void LogMessage(string message)
        {
            WriteLine("{0} | {1}", DateTime.Now, message);
        }

        public static void DebugMessage(string message)
        {
            LogMessage($"DEBUG: {message}");
        }
    }
}