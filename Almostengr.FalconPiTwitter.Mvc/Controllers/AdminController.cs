using Almostengr.FalconPiTwitter.Common.DataTransferObjects;
using Almostengr.FalconPiTwitter.Common.Extensions;
using Almostengr.FalconPiTwitter.Common.Services;
using Microsoft.AspNetCore.Mvc;

namespace Almostengr.FalconPiTwitter.Mvc.Controllers
{
    public class AdminController : Controller
    {
        private readonly IFptSettingsService _fptSettingsService;

        public AdminController(IFptSettingsService fptSettingsService)
        {
            _fptSettingsService = fptSettingsService;
        }

        // [HttpGet]
        // public async Task<IActionResult> GetFptSettings()
        // {
        //     return Ok(_fptSettingsService.GetFptSettings());
        // }

        // [HttpPost]
        // public async Task<IActionResult> UpsertFptSettings(FptSettingsDto fptSettingsDto)
        // {
        //     if (fptSettingsDto.IsNull())
        //     {
        //         return BadRequest();
        //     }

        //     _fptSettingsService.UpsertSettings(fptSettingsDto);

        //     return Ok();
        // }
        
    }
}