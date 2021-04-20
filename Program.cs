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
        public static void Main(string[] args)
        {
            foreach (var argument in args)
            {
                switch (argument)
                {
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
                        HelpConsoleCmd helpConsoleCmd = new HelpConsoleCmd();
                        helpConsoleCmd.Run();
                        break;

                    default:
                        CreateHostBuilder(args).Build().Run();
                        break;
                }
                // break; // prevents from ending up in loop
            }
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

                    services.AddHostedService<FppVitalsWorker>();

                    if (appSettings.MonitorOnly == false)
                    {
                        services.AddHostedService<FppCurrentSongWorker>();
                    }
                });
    }
}
