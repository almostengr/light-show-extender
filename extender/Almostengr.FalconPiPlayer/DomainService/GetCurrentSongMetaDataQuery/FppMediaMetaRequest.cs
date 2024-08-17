using Almostengr.Extensions;

namespace Almostengr.FalconPiPlayer.DomainService.FalconPiPlayer;

public sealed class MediaMetaRequest : IQueryRequest
{
    public MediaMetaRequest(string currentSong)
    {
        CurrentSong = currentSong;
    }

    public string CurrentSong { get; init; }
}