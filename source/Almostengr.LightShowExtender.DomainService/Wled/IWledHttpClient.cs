namespace Almostengr.LightShowExtender.DomainService.Wled;

public interface IWledHttpClient
{
    Task<WledJsonResponseDto> GetStatusAsync(string hostname);
    Task<WledJsonResponseDto> PostStateAsync(string hostname, WledJsonStateRequestDto wledRequestDto);
}