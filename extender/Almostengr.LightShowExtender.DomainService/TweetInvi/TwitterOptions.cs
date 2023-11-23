namespace Almostengr.LightShowExtender.DomainService.TweetInvi;

public sealed class TwitterOptions
{
    public string AccessSecret { get; init; } = string.Empty;
    public string AccessToken { get; init; } = string.Empty;
    public string ConsumerKey { get; init; } = string.Empty;
    public string ConsumerSecret { get; init; } = string.Empty;
    public uint HashTagCount { get; init; } = 3;
}