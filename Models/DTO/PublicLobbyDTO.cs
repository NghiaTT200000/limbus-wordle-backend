using Limbus_wordle_backend.Models;

namespace Limbus_wordle_backend.Models.DTO
{
    public class PublicLobbyDTO
    {
        public string LobbyCode { get; set; } = "";
        public int CurrentPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public TimeSpan GameLength { get; set; }
        public GameMode Mode { get; set; }
        public bool IsGameStarted { get; set; }
        public string HostName { get; set; } = "";
    }
}
