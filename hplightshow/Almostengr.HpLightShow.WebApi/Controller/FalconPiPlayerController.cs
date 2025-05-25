using Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Resources.DomainServices;
using Microsoft.AspNetCore.Mvc;

namespace Almostengr.HpLightShow.WebApi.Controller;

public sealed class FalconPiPlayerController : BaseApiController
{
    private readonly IFppdHttpClient _fppdHttpClient;
    private readonly IFppStartSequenceService _fppStartSeqeunceService;

    public FalconPiPlayerController(
        IFppdHttpClient fppdHttpClient,
        IFppStartSequenceService fppStartSequenceService
    )
    {
        _fppdHttpClient = fppdHttpClient;
        _fppStartSeqeunceService = fppStartSequenceService;
    }

    [HttpGet("status")]
    public async Task<IActionResult> Status()
    {
        var response = await _fppdHttpClient.GetStatusAsync();
        return Ok(response);
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartSequence(SequenceSelectorResource resource)
    {
        resource.CurrentDate = DateOnly.FromDateTime(DateTime.Now);

        Result<SequenceSelectorResource> startResult = await _fppStartSeqeunceService.ExecuteAsync(resource);
        if (startResult.Failed)
        {
            return BadRequest(startResult);
        }

        return Ok(startResult);
    }
}
