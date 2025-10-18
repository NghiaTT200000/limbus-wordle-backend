using System.Runtime.Serialization;

namespace Limbus_wordle_backend.Models
{
    public class GameLobby
    {
        public Guid Id { get; set; }
        public int MaxPlayers { get; set; } = 6;
        public bool IsPrivate { get; set; } = false;
        public TimeSpan GameLength { get; set; } = TimeSpan.FromMinutes(5);
        public GameMode Mode { get; set; } = GameMode.IdentityMode;
        public List<Player> Players { get; set; } = [];
        public List<Message> Messages { get; set; } = [];
        public bool IsGameStarted { get; set; } = false;
    }
    public enum GameMode
    {
        [EnumMember(Value = "IdentityMode")]
        IdentityMode,
    }
}