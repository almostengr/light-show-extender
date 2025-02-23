using Almostengr.FalconPiPlayerClient.DomainServices.Resources;

namespace Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;

public interface IEmailHttpClient
{
    Task<StatusMessageResource> SentTestEmailAsync();
}
