using System;

namespace Almostengr.FalconPiMonitor.ConsoleCmd
{
    public class BaseConsoleCmd
    {
        public string SystemdDirectory = "/lib/systemd/system";
        public string ServiceFilename = "falconpimonitor.service";
        public string AppDirectory =
            System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public void ConsoleMessage(string message)
        {
            Console.WriteLine("{0} {1}", DateTime.Now.ToString(), message);
        }
    }
}