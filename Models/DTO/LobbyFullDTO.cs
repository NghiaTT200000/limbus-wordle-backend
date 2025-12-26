using Limbus_wordle_backend.Models;

namespace Limbus_wordle_backend.Models.DTO
{
    public class LobbyFullDTO
    {
        public string LobbyCode { get; set; } = "";
        public bool IsPrivate { get; set; }
        public int MaxPlayers { get; set; }
        public TimeSpan GameLength { get; set; }
        public GameMode Mode { get; set; }
        public bool IsGameStarted { get; set; }
        public List<PlayerPublicDTO> Players { get; set; } = [];
        public List<MessageDTO> Messages { get; set; } = [];
        public TimeSpan? RemainingTime { get; set; }
    }
}
