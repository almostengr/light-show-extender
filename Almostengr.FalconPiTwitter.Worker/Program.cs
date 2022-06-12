using Almostengr.FalconPiTwitter.Clients;
using Almostengr.FalconPiTwitter.Common;
using Almostengr.FalconPiTwitter.Common.Constants;
using Almostengr.FalconPiTwitter.Common.Services;
using Almostengr.FalconPiTwitter.Services;
using Almostengr.FalconPiTwitter.Worker;
using Tweetinvi;

Console.WriteLine(typeof(Program).Assembly.ToString());

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile("appsettings.Development.json", true, true)
    .Build();

IHost host = Host.CreateDefaultBuilder(args)
    .UseSystemd()
    .UseContentRoot(
        System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location))
    .ConfigureServices(services =>
    {
        // APPSETTINGS ///////////////////////////////////////////////////////////////////////////////////////

        AppSettings appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

        if (appSettings.FppHosts.Count == 0)
        {
            appSettings.FppHosts.Add(AppConstants.Localhost);
        }

        // CLIENT ////////////////////////////////////////////////////////////////////////////////////////////

        services.AddTransient<IFppClient, FppClient>();
        services.AddTransient<ITwitterClient, TwitterClient>(tc =>
            new TwitterClient(
                appSettings.Twitter.ConsumerKey,
                appSettings.Twitter.ConsumerSecret,
                appSettings.Twitter.AccessToken,
                appSettings.Twitter.AccessSecret
            ));

        // SERVICES //////////////////////////////////////////////////////////////////////////////////////////

        services.AddTransient<ICountDownService, CountDownService>();
        services.AddTransient<IFppService, FppService>();
        services.AddTransient<ITwitterService, TwitterService>();

        // WORKERS ///////////////////////////////////////////////////////////////////////////////////////////

        services.AddHostedService<ChristmasCountDownWorker>();
        services.AddHostedService<FppCurrentSongWorker>();
        services.AddHostedService<FppVitalsWorker>();
        services.AddHostedService<LightShowCountdownWorker>();
    })
    .Build();

await host.RunAsync();
