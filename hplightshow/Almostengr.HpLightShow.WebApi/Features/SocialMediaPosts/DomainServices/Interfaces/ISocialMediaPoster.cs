namespace Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices.Interfaces;

public interface ISocialMediaPoster
{
    Task PostAsync(string message);
}