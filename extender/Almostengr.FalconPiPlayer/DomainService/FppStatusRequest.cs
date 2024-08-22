using Almostengr.Common.Query;

namespace Almostengr.FalconPiPlayer.DomainService;

public sealed class FppStatusRequest : IQueryRequest
{
    public string Hostname { get; init; }

    public FppStatusRequest(string hostname)
    {
        Hostname = hostname;
    }
}