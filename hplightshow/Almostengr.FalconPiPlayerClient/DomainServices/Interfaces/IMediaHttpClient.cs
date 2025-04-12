namespace Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;

public interface IMediaHttpClient
{
    Task<List<string>> GetMediaAsync();
    Task<MediaMetaResource> GetMediaMetaAsync(string mediaName);
}