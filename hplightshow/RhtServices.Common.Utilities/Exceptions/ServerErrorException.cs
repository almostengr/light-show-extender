using System.Net;

namespace RhtServices.Common.Utilities.Exceptions;

public sealed class ServerErrorException : Exception
{
    public ServerErrorException(HttpStatusCode statusCode, string body) :
        base($"Code: {statusCode}, Body: body")
    { }
}
