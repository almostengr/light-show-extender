namespace Almostengr.HpLightShow.Core.SocialMedias.DomainServices;

public interface ISocialMediaPoster
{
    Task PostAsync(string message);
}