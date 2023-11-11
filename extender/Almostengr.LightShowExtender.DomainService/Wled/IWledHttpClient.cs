namespace Almostengr.LightShowExtender.DomainService.Wled;

public interface IWledHttpClient
{
    Task<WledJsonResponse> GetStatusAsync(string hostname, CancellationToken cancellationToken);
    Task<WledJsonResponse> PostStateAsync(string hostname, WledJsonStateRequest wledRequestDto, CancellationToken cancellationToken);
}