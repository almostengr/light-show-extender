namespace Almostengr.LightShowExtender.DomainService.FalconPiPlayer;

public static class FppFeatureHandlers : IFeatureHandlers
{
    public static void AddHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetCpuTemperaturesHandler>();
        serviceCollection.AddScoped<GetCurrentSongMetaDataHandler>();
        serviceCollection.AddScoped<GetMultiSyncSystemsHandler>();
        serviceCollection.AddScoped<GetStatusHandler>();
        serviceCollection.AddScoped<InsertPlaylistAfterCurrentHandler>();
        serviceCollection.AddScoped<InsertPsaHandler>();
        serviceCollection.AddScoped<StopShowAfterEndTimeHandler>();
    }
}