using Almostengr.Common.Command;

namespace Almostengr.LightShowExtender.Worker.Twitter;

public sealed class PostTweetResponse : CommandResponse, ICommandResponse
{
    public PostTweetResponse(bool succeeded, object? data = null, string message = "") : base(succeeded, data, message)
    {
    }
}