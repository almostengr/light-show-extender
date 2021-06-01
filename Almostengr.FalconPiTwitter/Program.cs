using System;
using Almostengr.FalconPiTwitter.Models;
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
            if (args.Length > 0)
            {
                ShowHelp();
            }
            else
            {
                CreateHostBuilder(args).Build().Run();
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

                    services.AddSingleton<IFppVitalsWorker, FppVitalsWorker>();
                    // services.AddSingleton<IFppVitalsWorker, MockFppVitalsWorker>();

                    if (appSettings.MonitorOnly == false)
                    {
                        services.AddSingleton<IFppCurrentSongWorker, FppCurrentSongWorker>();
                        services.AddSingleton<ITwitterWorker, TwitterWorker>();
                        // services.AddSingleton<IFppCurrentSongWorker, MockFppCurrentSongWorker>();
                        // services.AddSingleton<ITwitterWorker, MockTwitterWorker>();
                    }
                });

        private static void ShowHelp()
        {
            Console.WriteLine("Falcon Pi Monitor Help");
            Console.WriteLine();
            Console.WriteLine("For more information about this program,");
            Console.WriteLine("visit https://thealmostengineer.com/falconpitwitter");
        }
    }
}
