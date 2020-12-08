using System.Threading.Tasks;

namespace Almostengr.FalconPiMonitor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            FppMonitor fppMonitor = new FppMonitor();
            await fppMonitor.RunMonitor();
        }
    }
}
