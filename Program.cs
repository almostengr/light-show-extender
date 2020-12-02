using System;

namespace Almostengr.FalconPiMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            PiMonitor piMonitor = new PiMonitor();
            while (true)
            {
                piMonitor.PerformChecks();
            }
        }
    }
}
