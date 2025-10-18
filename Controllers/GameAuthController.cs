
using Limbus_wordle_backend.Models.DTO;
using Limbus_wordle_backend.Services;
using Limbus_wordle_backend.Util.Environment;
using Microsoft.AspNetCore.Mvc;

namespace Limbus_wordle_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameAuthController(JwtService jwtService, AuthService authService) : ControllerBase
    {
        private readonly JwtService _jwtService = jwtService;
        private readonly AuthService _authService = authService;

        [HttpPost("issue-cookie")]
        public IActionResult IssueCookie([FromBody] PlayerCreateDTO req)
        {
            var token = _jwtService.CreateToken(req, TimeSpan.FromDays(100000));
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(100000)
            };
            Response.Cookies.Append(EnvironmentVariables.playerDataAuthCookie, token, cookieOptions);
            return Ok();
        }

        [HttpGet("get-player")]
        public IActionResult GetPlayer()
        {
            var player = _authService.GetTokenFromCookie(EnvironmentVariables.playerDataAuthCookie, EnvironmentVariables.playerDataClaimName);
            return Ok(player);
        }

    }
}