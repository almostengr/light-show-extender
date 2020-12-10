// using System;
// using System.IO;
using System;
using System.Threading.Tasks;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Hosting;
// using Serilog;

namespace Almostengr.FalconPiMonitor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // var builder = new ConfigurationBuilder();
            // BuildConfig(builder);

            // // serilog configuration
            // Log.Logger = new LoggerConfiguration()
            //     .ReadFrom.Configuration(builder.Build())
            //     .Enrich.FromLogContext()
            //     .WriteTo.Console()
            //     .CreateLogger();

            // var host = Host.CreateDefaultBuilder()
            //     .ConfigureServices((ContextBoundObject, services) => 
            //     {

            //     })
            //     .UseSerilog()
            //     .Build();

            // for (int i = 0; i < args.Length; i++)
            // {
            //     switch (args[i].ToLower())
            //     {
            //         case "--consumerkey":
            //             consumerKey = args[i + 1];
            //             break;

            //         case "--consumersecret":
            //             consumerSecret = args[i + 1];
            //             break;

            //         case "--accessToken":
            //             accessToken = args[i + 1];
            //             break;

            //         case "--accesssecret":
            //             accessSecret = args[i + 1];
            //             break;

            //         default:
            //             break;
            //     }
            // }

            FppMonitor fppMonitor = new FppMonitor();
            await fppMonitor.RunMonitor();
        }

        // static void BuildConfig(IConfigurationBuilder builder)
        // {
        //     // reference https://www.youtube.com/watch?v=GAOCe-2nXqc
        //     builder.SetBasePath(Directory.GetCurrentDirectory())
        //         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //         .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        //         .AddEnvironmentVariables();
        // }
    }
}
