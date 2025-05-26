using Almostengr.Common.DomainServices.Results;
using Almostengr.HpLightShow.WebApi.Features.StartSequence.DomainServices;
using Almostengr.HpLightShow.WebApi.Features.StartSequence.DomainServices.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Almostengr.HpLightShow.WebApi.Controller;

public sealed class FalconPiPlayerController : BaseApiController
{
    private readonly IStartSequenceService _fppStartSeqeunceService;

    public FalconPiPlayerController(
        IStartSequenceService fppStartSequenceService
    )
    {
        _fppStartSeqeunceService = fppStartSequenceService;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartSequence(SequenceSelectorResource resource)
    {
        resource.CurrentDate = DateOnly.FromDateTime(DateTime.Now);

        Result<SequenceSelectorResource> result = await _fppStartSeqeunceService.ExecuteAsync(resource);
        if (result.Failed)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
