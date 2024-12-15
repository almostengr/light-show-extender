using RhtServices.Common.Query;

namespace RhtServices.FalconPiPlayer.DomainService;

public sealed class FppStatusQuery : IQueryRequest
{
    public string Hostname { get; init; }

    public FppStatusQuery(string hostname)
    {
        Hostname = hostname;
    }
}