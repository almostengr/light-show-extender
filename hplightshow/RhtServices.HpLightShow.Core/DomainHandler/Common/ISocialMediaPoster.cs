namespace RhtServices.HpLightShow.Core.Common.DomainHandler.Common;

public interface ISocialMediaPoster
{
    Task PostAsync(string message);
}