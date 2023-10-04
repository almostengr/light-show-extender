namespace Almostengr.LightShowExtender.DomainService;

public interface IDisplayService
{
    Task<TimeSpan> UpdateWebsiteDisplayAsync();
}