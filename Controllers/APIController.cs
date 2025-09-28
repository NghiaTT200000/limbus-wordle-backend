

using System.IO.Abstractions;
using System.Text.Json;
using Limbus_wordle_backend.Models;
using Limbus_wordle_backend.Services;
using Limbus_wordle_backend.Util.Environment;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class APIController(DailyIdentityFileService dailyIdentityFileService, IdentityFileService identityFileService):ControllerBase{
    private readonly DailyIdentityFileService _dailyIdentityFileService = dailyIdentityFileService;
    private readonly IdentityFileService _identityFileService = identityFileService;

    [HttpGet("TodayIdentity")]
    public IActionResult TodayIdentity(){

        return Ok(_dailyIdentityFileService.GetDailyIdentityFile());
    }

    [HttpGet("All")]
    public async Task<IActionResult> All(){
        return Ok(await _identityFileService.getAllIdentities());
    }
}