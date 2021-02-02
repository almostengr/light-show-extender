using System;
using System.IO;
using System.Reflection;

namespace Almostengr.FalconPiMonitor.ConsoleCmd
{
    public class BuildConsoleCmd
    {
        public void Run()
        {
            Console.WriteLine("Build Date: {0}",
                File.GetCreationTime(Assembly.GetExecutingAssembly().Location));
        }
    }
}