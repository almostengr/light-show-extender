using Almostengr.Extensions;

namespace Almostengr.Common.NwsWeather;

public static class GetLatestObservationHandler
{
    public static async Task<NwsLatestObservationResponse> Handle(INwsHttpClient nwsHttpClient, string stationId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(stationId))
        {
            throw new ArgumentNullException(nameof(stationId));
        }

        return await nwsHttpClient.GetLatestObservationAsync(stationId, cancellationToken);
    }
}

public sealed class NwsLatestObservationResponse : BaseResponse
{
    public NwsLatestObservationProperties Properties { get; init; } = new();

    public sealed class NwsLatestObservationProperties
    {
        public NwsLatestObservationTemperature Temperature { get; init; } = new();
        public NwsLatestObservationWindChill WindChill { get; init; } = new();

        public sealed class NwsLatestObservationWindChill
        {
            public float? Value { get; init; } = null;
        }

        public sealed class NwsLatestObservationTemperature
        {
            public float? Value { get; init; } = null;
        }
    }
}
