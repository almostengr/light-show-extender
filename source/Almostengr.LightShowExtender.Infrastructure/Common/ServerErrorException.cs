using System.Net;

namespace Almostengr.LightShowExtender.Infrastructure.Common;

internal class ServerErrorException : Exception
{
    public ServerErrorException(HttpStatusCode statusCode, string body) :
        base($"Code: {statusCode}, Body: body")
    {
    }
}