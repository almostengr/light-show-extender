using Almostengr.HpLightShow.WebApi.Features.Monitoring.DomainServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Almostengr.HpLightShow.WebApi.Controller;

public class HealthController : BaseApiController
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
            var resource = await _fppHttpClient.GetStatusAsync();
            return Ok(resource);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
