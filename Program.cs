using Almostengr.FalconPiMonitor.ConsoleCmd;
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
            foreach (string argument in args)
            {
                switch (argument)
                {
                    case "--build":
                        BuildConsoleCmd buildConsoleCmd = new BuildConsoleCmd();
                        buildConsoleCmd.Run();
                        break;

                    case "--systemdon":
                        InstallSystemdConsoleCmd installSystemdConsoleCmd = new InstallSystemdConsoleCmd();
                        installSystemdConsoleCmd.Run();
                        break;

                    case "--systemdoff":
                        UninstallSystemdConsoleCmd uninstallSystemdConsoleCmd = new UninstallSystemdConsoleCmd();
                        uninstallSystemdConsoleCmd.Run();
                        break;

                    case "-h":
                    case "--help":
                    default:
                        HelpConsoleCmd helpConsoleCmd = new HelpConsoleCmd();
                        helpConsoleCmd.Run();
                        break;
                }
            }

            if (args.Length == 0)
            {
                CreateHostBuilder(args).Build().Run();
            }
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
                    services.AddHostedService<BaseService>();
                    services.AddHostedService<FppCurrentSongService>();
                    services.AddHostedService<FppVitalsService>();
                    // services.AddHostedService<WeatherService>();
                    // services.AddHostedService<TwitterRepliesService>();
                });
    }
}
