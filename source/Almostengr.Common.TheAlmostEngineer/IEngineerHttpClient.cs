namespace Almostengr.Common.TheAlmostEngineer;

public interface IEngineerHttpClient
{
    Task<EngineerResponseDto> GetFirstUnplayedRequestAsync();
    Task DeleteAllSongsInQueueAsync();
    Task<EngineerResponseDto> PostDisplayInfoAsync(EngineerDisplayRequestDto requestDto);
}