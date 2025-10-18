using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Limbus_wordle_backend.Models;
using Limbus_wordle_backend.Models.DTO;
using Limbus_wordle_backend.Util.Environment;
using Microsoft.IdentityModel.Tokens;

namespace Limbus_wordle_backend.Services
{
    public class JwtService
    {

        public string CreateToken(PlayerCreateDTO playerCreateDTO, TimeSpan? expires = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentVariables.jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;

            var player = new Player(playerCreateDTO);
            var playerJson = JsonSerializer.Serialize(player);

            var token = new JwtSecurityToken(
                issuer: EnvironmentVariables.jwtIssuer,
                claims: [new Claim(EnvironmentVariables.playerDataAuthCookie, 
                    playerJson)],
                notBefore: now,
                expires: now.Add(expires ?? TimeSpan.FromDays(1)),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(EnvironmentVariables.jwtSecret);
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = EnvironmentVariables.jwtIssuer,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                }, out _);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}