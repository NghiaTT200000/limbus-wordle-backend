using System.Text.Json;
using Limbus_wordle_backend.Models;
using Limbus_wordle_backend.Models.Exceptions;

namespace Limbus_wordle_backend.Services
{
    public class AuthService(JwtService jwtService, HttpContext httpContext)
    {
        private readonly JwtService _jwtService = jwtService;
        private readonly HttpContext _httpContext = httpContext;

        public Player? GetTokenFromCookie(string cookieName,string claimName)
        {
            try
            {
                var token = _httpContext.Request.Cookies[cookieName];
                if (!string.IsNullOrEmpty(token))
                {
                    var principal = _jwtService.ValidateToken(token);
                    if (principal != null)
                    {
                        var claimJson = principal?.FindFirst(claimName)?.Value;
                        if(string.IsNullOrEmpty(claimJson)) return null;
                        return JsonSerializer.Deserialize<Player>(claimJson);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to validate game auth cookie: {ex}");
            }
            throw new NoPlayerDataException();
        }
    }
}