using System.Runtime.Serialization;
using Limbus_wordle_backend.Interfaces;
using Limbus_wordle_backend.Models.DTO;
using Limbus_wordle_backend.Services.GameLoop;

namespace Limbus_wordle_backend.Models
{
    public class GameLobby
    {
        public string LobbyCode { get; set; } = "";
        public int MaxPlayers { get; set; } = 6;
        public bool IsPrivate { get; set; } = false;
        public TimeSpan GameLength { get; set; } = TimeSpan.FromMinutes(2);
        public GameMode Mode { get; set; } = GameMode.IdentityMode;
        public IGameLoop GameLoop { get; set; }
        public List<Player> Players { get; set; } = [];
        public List<Message> Messages { get; set; } = [];
        public bool IsGameStarted { get; set; } = false;

        public DateTime? StartTimeUtc  { get; set; }
        public DateTime? EndTimeUtc { get; set; }

        public TimeSpan? RemainingTimeUtc => EndTimeUtc.HasValue ? EndTimeUtc.Value - DateTime.UtcNow : null;

        public GameLobby(GameLobbyCreateDTO createDTO)
        {
            MaxPlayers = createDTO.MaxPlayers;
            IsPrivate = createDTO.IsPrivate;
            GameLength = createDTO.GameLength;
            Mode = createDTO.Mode;
            LobbyCode = GenerateLobbyCode();

            GameLoop = createDTO.Mode switch
            {
                GameMode.IdentityMode => new IdentityModeGameLoopService(),
                _ => throw new Exception("GameMode type does not exist"),
            };
        }

        private static string GenerateLobbyCode()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";  // No I, O, 0, 1
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
    public enum GameMode
    {
        [EnumMember(Value = "IdentityMode")]
        IdentityMode,
    }
}