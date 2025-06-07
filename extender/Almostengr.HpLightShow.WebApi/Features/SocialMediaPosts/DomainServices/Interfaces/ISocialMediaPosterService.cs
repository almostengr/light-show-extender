using Almostengr.Common.DomainServices.Interfaces;
using Almostengr.Common.DomainServices.Results;
using Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices.Resources;

namespace Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices.Interfaces;

public interface ISocialMediaPosterService : ICommandService<SocialMediaResource>
{
    Task<Result<SocialMediaResource>> PostAsync(string message);
}