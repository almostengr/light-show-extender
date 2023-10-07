namespace Almostengr.LightShowExtender.DomainService;

public interface IExtenderService
{
    Task<TimeSpan> UpdateWebsiteDisplayAsync();
}