using System.ComponentModel.DataAnnotations;

namespace Limbus_wordle_backend.Models.DTO
{
    public class GameLobbyCreateDTO
    {
        [Range(1, 6, ErrorMessage = "The max players must be between 1 and 6")]
        public int MaxPlayers { get; set; } = 6;
        public bool IsPrivate { get; set; } = false;
        [Range(typeof(TimeSpan), "00:01:00", "00:05:00", ErrorMessage = "Game length must be from 1 to 5 minutes")]
        public TimeSpan GameLength { get; set; } = TimeSpan.FromMinutes(1);
        public GameMode Mode { get; set; } = GameMode.IdentityMode;
    }
}