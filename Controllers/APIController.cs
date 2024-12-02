

using System.IO.Abstractions;
using System.Text.Json;
using Limbus_wordle.Services;
using Limbus_wordle.util.Functions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class APIController(IDataProtectionProvider dataProtectorProvider):ControllerBase{
    private readonly IDataProtector _dataProtector = dataProtectorProvider.CreateProtector(Environment.GetEnvironmentVariable("CookiePass"));

    [HttpGet("TodayIdentity")]
    public async Task<IActionResult> TodayIdentity(){

        return Ok(DailyIdentityGameModeService.GetDailyIdentityFile());
    }

    [HttpGet("Random")]
    public async Task<IActionResult> Random(){
        return Ok(RandomIdentity.Get());
    }

    [HttpGet("All")]
    public async Task<IActionResult> All(){
        var rootLink = Directory.GetCurrentDirectory();
        var identitiesFilePath = Path.Combine(rootLink, Environment.GetEnvironmentVariable("IdentityJSONFile"));

        string identitiesFile = await new FileSystem().File.ReadAllTextAsync(identitiesFilePath);

        var deserializeIdentities = JsonSerializer.Deserialize<Dictionary<string,Identity>>(identitiesFile)
            ??new Dictionary<string,Identity>();
        
        return Ok(deserializeIdentities);
    }
}