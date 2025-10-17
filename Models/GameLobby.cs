using System.Runtime.Serialization;

namespace Limbus_wordle_backend.Models
{
    public class GameLobby<GuessModel>
    {
        public Guid Id { get; set; }
        public int MaxPlayers { get; set; } = 4;
        public bool IsPrivate { get; set; } = false;
        public TimeSpan GameLength { get; set; } = TimeSpan.FromMinutes(5);
        public GameMode Mode { get; set; } = GameMode.IdentityMode;
        public List<Player<GuessModel>> Players { get; set; } = new List<Player<GuessModel>>();
        public List<Message> Messages { get; set; } = new List<Message>();
        public bool IsGameStarted { get; set; } = false;
    }
    public enum GameMode
    {
        [EnumMember(Value = "IdentityMode")]
        IdentityMode,
    }
}