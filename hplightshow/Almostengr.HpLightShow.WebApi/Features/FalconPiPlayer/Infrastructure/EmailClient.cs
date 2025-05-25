using Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;
using Almostengr.FalconPiPlayerClient.DomainServices.Resources;

namespace Almostengr.FalconPiPlayerClient.Infrastructure;

public sealed class EmailHttpClient : BaseClient, IEmailHttpClient
{
    public EmailHttpClient(HttpClient httpClient) : base(httpClient)
    {
    }    

    public async Task<StatusMessageResource> SentTestEmailAsync()
    {
        var response = await _httpClient.PostAsync("api/email/test", null);
        var result = await DeserializeResponseBodyAsync<StatusMessageResource>(response);
        return result;
    }
}
