namespace Limbus_wordle_backend.Models.DTO
{
    public class JoinResultDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public LobbyFullDTO? Lobby { get; set; }
    }
}
