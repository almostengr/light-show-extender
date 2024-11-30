using System.Text;
using Almostengr.Common.Command;
using Almostengr.LightShowExtender.DomainService.Twitter;

namespace Almostengr.LightShowExtender.Worker.DomainService;

public sealed class HolidayCountdownCommandHandler : ICommandHandler<HolidayCountdownCommand, HolidayCountdownResponse>
{
    private readonly TwitterAppSettings _twitterAppSettings;

    public HolidayCountdownCommandHandler(TwitterAppSettings twitterAppSettings)
    {
        _twitterAppSettings = twitterAppSettings;
    }

    public async Task<HolidayCountdownResponse> ExecuteAsync(CancellationToken cancellationToken, HolidayCountdownCommand command)
    {
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        DateOnly christmasDateTime = DateOnly.FromDateTime(new DateTime(DateTime.Now.Year, 12, 25));
        int daysDifference = christmasDateTime.DayNumber - currentDate.DayNumber;

        StringBuilder tweet = new();
        if (daysDifference > 0)
        {
            string text = daysDifference == 1 ? $"1 day " : $"{daysDifference} days ";
            tweet.Append(text);

            tweet.Append(" until Christmas!");
        }
        else if (daysDifference == 0)
        {
            tweet.Append("Today is Christmas!");
        }

        if (tweet.Length > 0)
        {
            var handler = new PostTweetCommandHandler(_twitterAppSettings);
            var tweetCommand = new PostTweetCommand(tweet.ToString());
            await handler.ExecuteAsync(cancellationToken, tweetCommand);
        }

        return new HolidayCountdownResponse(true);
    }
}
