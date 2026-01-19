using Limbus_wordle_backend.Services.IdentityFile;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class APIController(IdentityFileService identityFileService):ControllerBase{
    private readonly IdentityFileService _identityFileService = identityFileService;

    [HttpGet("TodayIdentity")]
    public IActionResult TodayIdentity(){

        return Ok(_identityFileService.GetDailyIdentityFile());
    }

    [HttpGet("All")]
    public async Task<IActionResult> All(){
        return Ok(await _identityFileService.GetAllIdentities());
    }
}