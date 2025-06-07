using Almostengr.Common.DomainServices.Results;
using Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices.Interfaces;
using Almostengr.HpLightShow.WebApi.Features.SocialMediaPosts.DomainServices.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Almostengr.HpLightShow.WebApi.Controller;

public class SocialMediaController : BaseApiController
{
    private readonly ISocialMediaPosterService _socialMediaPosterService;

    public SocialMediaController
    (
        ISocialMediaPosterService socialMediaPosterService
    )
    {
        _socialMediaPosterService = socialMediaPosterService;
    }

    [HttpPost]
    public async Task<IActionResult> Post(SocialMediaResource resource)
    {
        Result<SocialMediaResource> result = await _socialMediaPosterService.ExecuteAsync(resource);
        if (result.Failed)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}