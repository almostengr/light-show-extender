using System;
using System.Net.Http;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.ConsoleCmd;
using Almostengr.FalconPiTwitter.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tweetinvi;

namespace Almostengr.FalconPiTwitter
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length == 1)
            {

                switch (args[0])
                {
                    case "--systemdon":
                        InstallSystemdConsoleCmd installSystemdConsoleCmd = new InstallSystemdConsoleCmd();
                        installSystemdConsoleCmd.Run();
                        break;

                    case "--systemdoff":
                        UninstallSystemdConsoleCmd uninstallSystemdConsoleCmd = new UninstallSystemdConsoleCmd();
                        uninstallSystemdConsoleCmd.Run();
                        break;

                    case "--service":
                        break;

                    case "-h":
                    case "--help":
                        HelpConsoleCmd helpConsoleCmd = new HelpConsoleCmd();
                        helpConsoleCmd.Run();
                        break;

                    default:
                        CreateHostBuilder(args).Build().Run();
                        break;
                }

                Console.WriteLine("Press ENTER key to exit...");
                Console.ReadLine();
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
                    services.AddSingleton<IFppCurrentSongWorker, FppCurrentSongWorker>();
                    // services.AddSingleton<ITwitterClient, TwitterClient>();
                    services.AddSingleton<ITwitterClient>(tc =>
                       new TwitterClient(
                           "key",
                           "secret",
                           "accestoken",
                           "accesssecret"
                       ));
                    services.AddSingleton<HttpClient>();
                });
    }
}
