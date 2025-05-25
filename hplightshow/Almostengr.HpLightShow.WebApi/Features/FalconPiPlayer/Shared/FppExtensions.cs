using Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;
using Almostengr.FalconPiPlayerClient.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.FalconPiPlayer.HttpClient.Common;

public static class FppExtensions
{
    public static void AddFppClient(this IServiceCollection serviceCollection, string baseAddress = "http://127.0.0.1")
    {
        serviceCollection.AddHttpClient<IFppdHttpClient, FppdClient>(client =>
        {
            client.BaseAddress = new Uri(baseAddress);
            client.Timeout = TimeSpan.FromSeconds(15);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
    }
}