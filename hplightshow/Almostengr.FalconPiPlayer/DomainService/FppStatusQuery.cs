using Almostengr.Common.Query;

namespace Almostengr.FalconPiPlayer.DomainService;

public sealed class FppStatusQuery : IQueryRequest
{
    public string Hostname { get; init; }

    public FppStatusQuery(string hostname)
    {
        Hostname = hostname;
    }
}