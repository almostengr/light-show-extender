using Microsoft.Extensions.DependencyInjection;

namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public static class FppFeatureHandlers
{
    public static void AddHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<GetCpuTemperaturesHandler>();
        serviceCollection.AddSingleton<GetCurrentSongMetaDataHandler>();
        serviceCollection.AddSingleton<GetMultiSyncSystemsHandler>();
        serviceCollection.AddSingleton<GetSequenceListHandler>();
        serviceCollection.AddSingleton<GetStatusHandler>();
        serviceCollection.AddSingleton<InsertPlaylistAfterCurrentHandler>();
        serviceCollection.AddSingleton<InsertPsaHandler>();
        serviceCollection.AddSingleton<StopShowAfterEndTimeHandler>();
    }
}