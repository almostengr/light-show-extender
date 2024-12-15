using RhtServices.Common.Query;

namespace RhtServices.FalconPiPlayer.DomainService;

public sealed class MediaMetaRequest : IQueryRequest
{
    public MediaMetaRequest(string currentSong)
    {
        CurrentSong = currentSong;
    }

    public string CurrentSong { get; init; }
}