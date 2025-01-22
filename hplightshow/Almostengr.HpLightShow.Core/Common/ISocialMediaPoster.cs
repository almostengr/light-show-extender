namespace Almostengr.HpLightShow.Core.Common.Common;

public interface ISocialMediaPoster
{
    Task PostAsync(string message);
}