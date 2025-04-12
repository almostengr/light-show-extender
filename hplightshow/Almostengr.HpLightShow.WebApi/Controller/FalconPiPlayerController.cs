using Almostengr.Common.DomainServices.Results;
using Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;
using Almostengr.HpLightShow.Core.FalconPiPlayer.DomainServices;
using Almostengr.HpLightShow.Core.FalconPiPlayer.Resources.DomainServices;
using Microsoft.AspNetCore.Mvc;

namespace Almostengr.HpLightShow.WebApi;

[ApiController]
[Route("[controller]")]
public sealed class FalconPiPlayerController : ControllerBase
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

    [HttpGet]
    public async Task<IActionResult> Status()
    {
        var response = await _fppdHttpClient.GetStatusAsync();
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> StartSequence()
    {
        SequenceSelectorResource resource = new(DateOnly.FromDateTime(DateTime.Now));

        Result<SequenceSelectorResource> startResult = await _fppStartSeqeunceService.ExecuteAsync(resource);
        if (startResult.Failed)
        {
            return BadRequest(startResult);
        }

        return Ok(startResult);
    }
}
