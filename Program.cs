using System;
using System.Net.Http;
using System.Threading.Tasks;
using Almostengr.FalconPiMonitor.ConsoleCmd;
using Almostengr.FalconPiMonitor.Models;
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
            // if (args.Length == 1)
            // {

            //     switch (args[0])
            //     {
            //         case "--systemdon":
            //             InstallSystemdConsoleCmd installSystemdConsoleCmd = new InstallSystemdConsoleCmd();
            //             installSystemdConsoleCmd.Run();
            //             break;

            //         case "--systemdoff":
            //             UninstallSystemdConsoleCmd uninstallSystemdConsoleCmd = new UninstallSystemdConsoleCmd();
            //             uninstallSystemdConsoleCmd.Run();
            //             break;

            //         case "--service":
            //             CreateHostBuilder(args).Build().Run();
            //             break;

            //         case "-h":
            //         case "--help":
            //             HelpConsoleCmd helpConsoleCmd = new HelpConsoleCmd();
            //             helpConsoleCmd.Run();
            //             break;

            //         default:
            //             CreateHostBuilder(args).Build().Run();
            //             break;
            //     }

            //     Console.WriteLine("Press ENTER key to exit...");
            //     Console.ReadLine();
            // }
            // else
            // {
            //     Console.WriteLine("Too many arguments specified");
            //     Console.WriteLine();
            //     HelpConsoleCmd helpConsoleCmd = new HelpConsoleCmd();
            //     helpConsoleCmd.Run();
            // }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    AppSettings appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();
                    services.AddSingleton(appSettings);

                    services.AddSingleton<ITwitterClient, TwitterClient>(tc => 
                        new TwitterClient(
                            appSettings.Twitter.ConsumerKey,
                            appSettings.Twitter.ConsumerSecret,
                            appSettings.Twitter.AccessToken,
                            appSettings.Twitter.AccessSecret
                        ));

                    // services.AddHostedService<FppVitalsWorker>();
                    services.AddHostedService<FppCurrentSongWorker>();
                });
    }
}
