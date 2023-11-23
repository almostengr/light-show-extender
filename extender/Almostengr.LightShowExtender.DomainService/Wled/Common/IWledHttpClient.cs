namespace Almostengr.LightShowExtender.DomainService.Wled;

public interface IWledHttpClient
{
    Task<WledJsonStateResponse> GetStatusAsync(string hostname, CancellationToken cancellationToken);
    Task<WledJsonStateResponse> PostStateAsync(string hostname, WledJsonStateRequest wledRequestDto, CancellationToken cancellationToken);
}