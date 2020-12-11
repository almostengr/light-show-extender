using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Almostengr.FalconPiMonitor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            AppSettings cfg = config.Get<AppSettings>();

            FppMonitor fppMonitor = new FppMonitor(cfg);
            await fppMonitor.RunMonitor();
        }
    }
}
