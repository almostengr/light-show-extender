using Almostengr.FalconPiPlayerClient.DomainServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Almostengr.HpLightShow.WebApi.Controller;

public sealed class HealthController : BaseApiController
{
    private readonly IFppdHttpClient _fppHttpClient;

    public HealthController(
        IFppdHttpClient fppdHttpClient
    )
    {
        _fppHttpClient = fppdHttpClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            await _fppHttpClient.GetStatusAsync();
            return Ok("OK");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
