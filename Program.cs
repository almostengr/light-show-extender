using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using static Almostengr.FalconPiMonitor.Logger;

namespace Almostengr.FalconPiMonitor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            LogMessage(string.Concat("ENVIRONMENT: ", env));

            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            var cfg = config.Get<AppSettings>();

            Console.WriteLine(cfg.TwitterConfig.AccessSecret);

            // FppMonitor fppMonitor = new FppMonitor();
            // await fppMonitor.RunMonitor();
        }
    }
}
