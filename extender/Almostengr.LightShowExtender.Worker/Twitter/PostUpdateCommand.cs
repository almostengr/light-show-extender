using System.Text;
using Almostengr.Common;
using Almostengr.LightShowExtender.DomainService.Common;

namespace Almostengr.LightShowExtender.DomainService.Twitter;

public sealed class PostUpdateCommand : ICommand
{
    public PostUpdateCommand(string title, string artist)
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

    public PostUpdateCommand(string text)
    {
        Text = text;
    }

    public string Text { get; init; }
}
