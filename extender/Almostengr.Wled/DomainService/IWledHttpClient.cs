namespace Almostengr.Wled.DomainService

public interface IWledHttpClient
{
    Task<WledJsonStateResponse> GetStatusAsync(string hostname, CancellationToken cancellationToken);
    Task<WledJsonStateResponse> PostStateAsync(string hostname, WledJsonStateRequest wledRequestDto, CancellationToken cancellationToken);
}