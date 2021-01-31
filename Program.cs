using Almostengr.FalconPiMonitor.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Almostengr.FalconPiMonitor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .UseContentRoot(
                    System.IO.Path.GetDirectoryName(
                        System.Reflection.Assembly.GetExecutingAssembly().Location))
                .ConfigureAppConfiguration(
                    builder => new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    .AddEnvironmentVariables()
                )
                .ConfigureServices((hostContext, services) =>
                {
                    // services.AddHostedService<FppMonitorService>();
                    services.AddHostedService<FppVitalsService>();
                    services.AddHostedService<FppCurrentSongService>();
                    // services.AddHostedService<FppWeatherService>();
                    // services.AddHostedServices<FppTwitterRepliesService>();
                })
                ;
    }
}
