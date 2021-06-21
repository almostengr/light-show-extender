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
                ProcessArguments(args);
            }
            else
            {
                CreateHostBuilder(args).Build().Run();
            }
        }

        private static void ProcessArguments(string[] args)
        {
            switch (args[0])
            {
                case "--help":
                case "-h":
                case "help":
                    ShowHelp();
                    break;

                case "--version":
                case "version":
                    ShowVersion();
                    break;

                case "--fullversion":
                    ShowFullVersion();
                    break;
                    
                default:
                    Console.WriteLine("Invalid arguments");
                    ShowHelp();
                    break;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .UseContentRoot(
                    System.IO.Path.GetDirectoryName(
                        System.Reflection.Assembly.GetExecutingAssembly().Location))
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
                        services.AddHostedService<TwitterWorker>();
                        services.AddHostedService<CountdownWorker>();
                    }
                });

        private static void ShowFullVersion()
        {
            Console.WriteLine(typeof(Program).Assembly.ToString());
        }

        private static void ShowVersion()
        {
            Console.WriteLine(typeof(Program).Assembly.GetName().Version.ToString());
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Falcon Pi Twitter Help");
            Console.WriteLine();
            Console.WriteLine("For more information about this program,");
            Console.WriteLine("visit https://thealmostengineer.com/falconpitwitter");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(typeof(Program).Assembly.ToString());
        }
    }
}
