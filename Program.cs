using System;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.ConsoleCmd;
using Almostengr.FalconPiMonitor.Services;
using Almostengr.FalconPiMonitor.ServicesBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Almostengr.FalconPiMonitor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length == 1)
            {

                switch (args[0])
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

                    case "--weather":
                        WeatherAlertTypesConsoleCmd weatherAlertTypesConsoleCmd = new WeatherAlertTypesConsoleCmd();
                        await weatherAlertTypesConsoleCmd.RunAsync();
                        break;

                    case "-h":
                    case "--help":
                    default:
                        HelpConsoleCmd helpConsoleCmd = new HelpConsoleCmd();
                        helpConsoleCmd.Run();
                        break;
                }

                Console.WriteLine("Press ENTER key to exit...");
                Console.ReadLine();
            }
            else if (args.Length == 0)
            {
                CreateHostBuilder(args).Build().Run();
            }
            else
            {
                Console.WriteLine("Too many arguments specified");
                Console.WriteLine();
                HelpConsoleCmd helpConsoleCmd = new HelpConsoleCmd();
                helpConsoleCmd.Run();
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
                    services.AddHostedService<FppCurrentSongService>();
                    services.AddHostedService<FppVitalsService>();
                    services.AddHostedService<BaseService>();
                    // services.AddHostedService<WeatherService>();
                    // services.AddHostedService<TwitterRepliesService>();
                });
    }
}
