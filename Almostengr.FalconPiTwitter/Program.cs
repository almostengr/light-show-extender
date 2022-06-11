using System;
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
            ShowFullVersion();

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

                default:
                    Console.WriteLine("Invalid argument(s)");
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

                    // AppSettings appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

                    // if (appSettings.FppHosts.Count == 0)
                    // {
                    //     // appSettings.FppHosts.Add(AppConstants.Localhost);
                    // }

                    // services.AddSingleton(appSettings);

                    // // CLIENTS ///////////////////////////////////////////////////////////////////////////////
                    // //////////////////////////////////////////////////////////////////////////////////////////

                    // services.AddSingleton<ITwitterClient, TwitterClient>(tc =>
                    //     new TwitterClient(
                    //         appSettings.Twitter.ConsumerKey,
                    //         appSettings.Twitter.ConsumerSecret,
                    //         appSettings.Twitter.AccessToken,
                    //         appSettings.Twitter.AccessSecret
                    //     ));

                    // services.AddSingleton<IFppClient, FppClient>();

                    // // SERVICES //////////////////////////////////////////////////////////////////////////////
                    // //////////////////////////////////////////////////////////////////////////////////////////

                    // services.AddSingleton<IFppService, FppService>();
                    // services.AddSingleton<ITwitterService, TwitterService>();

                    // // WORKERS ///////////////////////////////////////////////////////////////////////////////
                    // //////////////////////////////////////////////////////////////////////////////////////////

                    // services.AddHostedService<FppVitalsWorker>();
                    // services.AddHostedService<FppCurrentSongWorker>();
                    // services.AddHostedService<CountdownWorker>();
                });

        private static void ShowFullVersion()
        {
            Console.WriteLine(typeof(Program).Assembly.ToString());
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Falcon Pi Twitter Help");
            Console.WriteLine();
            Console.WriteLine("For more information about this program,");
            Console.WriteLine("visit https://thealmostengineer.com/projects/falcon-pi-twitter");
            Console.WriteLine();
        }

    }
}
