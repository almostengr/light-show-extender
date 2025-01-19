namespace Almostengr.HpLightShow.Core.Countdowns;

public sealed class CountdownService
{
    public async Task<HandlerResult> ChristmasAsync(ChristmasCountdownDto countdownDto)
    {
        string? message = null;

        int daysDifference = countdownDto.ChristmasDate.DayNumber - countdownDto.CurrentDate.DayNumber;
        if (daysDifference > 0)
        {
            message = daysDifference == 1 ? $"1 day " : $"{daysDifference} days ";
        }
        else if (daysDifference == 0)
        {
            message = "Today is Christmas!";
        }

        if (!string.IsNullOrWhiteSpace(message))
        {
            await _socialMediaPoster.PostAsync(message);
        }

        return new HandlerResult(true);
    }

    public async Task<HandlerResult> ExecuteAsync(ShowCountdownDto countdownDto)
    {
        // todo separate dates for Christmas and 4th of July in configuraiton file

        if (countdownDto.IsChrismtas)
        {
            await ChristmasCountdownAsync(countdownDto);
            return new HandlerResult(true);
        }

        await IndependenceCountdownAsync(countdownDto);
        return new HandlerResult(true);
    }

    private async Task ChristmasCountdownAsync(ShowCountdownDto countdownDto)
    {

    }

    private async Task IndependenceCountdownAsync(ShowCountdownDto countdownDto)
    {

    }
}