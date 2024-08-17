namespace Almostengr.LightShowExtender.Worker;

public sealed class TwitterSettings
{
    public string AccessSecret { get; init; } = string.Empty;
    public string AccessToken { get; init; } = string.Empty;
    public string ConsumerKey { get; init; } = string.Empty;
    public string ConsumerSecret { get; init; } = string.Empty;
    public uint HashTagCount { get; init; } = 3;
    public uint CharacterLimit { get; init; } = 280;
}