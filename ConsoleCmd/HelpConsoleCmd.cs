using System;

namespace Almostengr.FalconPiMonitor.ConsoleCmd
{
    public class HelpConsoleCmd
    {
        public void Run()
        {
            Console.WriteLine("Falcon Pi Monitor Help");
            Console.WriteLine();
            Console.WriteLine("Usage: falconpimonitor [--build][--systemdon][--systemdoff][--help]");
            Console.WriteLine();
            Console.WriteLine("Runtime Ooptions:");
            Console.WriteLine("{0,-2}{1,-15}{2,-50}", "", "--build", "Displays the application build date");
            Console.WriteLine("{0,-2}{1,-15}{2,-50}", "", "-h | --help", "This help screen");
            Console.WriteLine("{0,-2}{1,-15}{2,-50}", "", "--systemdoff", "Uninstalls the program as Linux Service/System Daemon");
            Console.WriteLine("{0,-2}{1,-15}{2,-50}", "", "--systemdon", "Installs the program as Linux Service/System Daemon");
            Console.WriteLine("{0,-2}{1,-15}{2,-50}", "", "--weather", "Displays the weather alert types from the National Weather Service");
            Console.WriteLine();
            Console.WriteLine("For more information about this program,");
            Console.WriteLine("visit https://github.com/almostengr/falconpimonitor");
        }
    }
}