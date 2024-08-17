using Almostengr.Extensions;

namespace Almostengr.FalconPiPlayer.DomainService.FalconPiPlayer;

public sealed class FppStatusRequest : IQueryRequest
{
    public string Hostname { get; init; }

    public FppStatusRequest(string hostname)
    {
        Hostname = hostname;
    }
}