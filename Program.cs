namespace Almostengr.FalconPiMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            PiMonitor piMonitor = new PiMonitor();

            while (true)
            {
                piMonitor.PerformChecks();
            }
        }
    }
}
