using Almostengr.Common.Query;

namespace Almostengr.FalconPiPlayer.DomainService;

public sealed class MediaMetaRequest : IQueryRequest
{
    public MediaMetaRequest(string currentSong)
    {
        CurrentSong = currentSong;
    }

    public string CurrentSong { get; init; }
}