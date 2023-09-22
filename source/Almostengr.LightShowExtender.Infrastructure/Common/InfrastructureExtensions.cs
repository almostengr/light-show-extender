using System.Net;

namespace Almostengr.LightShowExtender.Infrastructure.Common;

internal static class InfrastructureExtensions
{
    internal static async Task<HttpResponseMessage> WasRequestSuccessfulAsync(this HttpResponseMessage response)
    {
        if (response.StatusCode >= HttpStatusCode.InternalServerError ||
            response.StatusCode == HttpStatusCode.RequestTimeout)
        {
            string body = await response.Content.ReadAsStringAsync()!;
            throw new ServerErrorException(response.StatusCode, body);
        }

        response.EnsureSuccessStatusCode();
        return response;
    }
}