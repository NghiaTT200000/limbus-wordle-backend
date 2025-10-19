namespace Limbus_wordle_backend.Models.DTO
{
    public class GameLobbyUpdateDTO
    {
        public int MaxPlayers { get; set; } = 6;
        public TimeSpan GameLength { get; set; } = TimeSpan.FromMinutes(2);
    }
}