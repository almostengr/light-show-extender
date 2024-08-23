using System.Text;
using Almostengr.Common;
using Almostengr.Common.Command;

namespace Almostengr.LightShowExtender.DomainService.Twitter;

public sealed class PostTweetCommand : ICommandRequest
{
    public PostTweetCommand(string title, string artist)
    {
        StringBuilder text = new StringBuilder();

        text.Append("Playing ");
        text.Append(title);

        if (artist.IsNotNullOrWhiteSpace())
        {
            text.Append(" by ");
            text.Append(artist);
        }

        Text = text.ToString();
    }

    public PostTweetCommand(string text)
    {
        Text = text;
    }

    public string Text { get; init; }
}
