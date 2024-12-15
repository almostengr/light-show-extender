namespace Almostengr.HpLightShow.DomainService.Common;

public interface ISocialMediaPoster
{
    Task PostAsync(string message);
}